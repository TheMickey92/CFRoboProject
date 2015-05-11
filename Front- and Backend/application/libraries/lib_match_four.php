<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

include_once 'lib_robot.php';

class lib_match_four extends lib_robot
{
    public function __construct()
    {        
        parent::__construct();
        $this->load->model('match_four_model');
    }
    
    /**
     * 
     * @throws Exception
     */
    public function run_algorithm()
    {           
        return $this->logic($this->get_last_field(), $this->get_field());
    }
    
    public function add_chip( $x, $y, $color )
    {
        $this->x($x)->y($y)->value( $color )->save();
    }
    
    public function remove_chip( $x, $y )
    {
        $config = $this->config->item('algorithm', 'match_four');
        $field = $this->get_field();
        while( $y > 0 )
        {
            $field[$x][$y] = $field[$x][$y - 1];
            $y--;
        }
        $field[$x][$y] = $config['values']['empty'];
        $this->set_field( $field );
    }
    
    public function robot_turn( $x )
    {
        $config = $this->config->item('hardware', 'match_four');
        
        //get vision camera
        $cam_name = $config['camera'];
        $list = $this->get_device_list();
        $camera = array_search($cam_name, $list );

        if( false === $camera )
            throw new Exception( "Camera '$cam_name' not found. Available: ".json_encode ( $list ));
        
        //get vision
        $before = json_decode( $this->get_view_of_device($camera) );
        
        //robot insert into field
        $this->insert_into( $x );
        
        //get vision 
        $after = json_decode( $this->get_view_of_device($camera) );
        
        //run algorithm
        $result = $this->logic($before, $after);
        
        if( !$result->is_valid() )
        {
            return;
        }
        
        //robot insert into field
        $this->insert_into( $result->x );
        
        if( !$result->has_finished() ) //call opponent
        {
            $this->send_to_opponent($result->x);
        }
    }
    
    public function startup()
    {
        $this->insert_into(3);
        $this->send_to_opponent(3);
    }
    
    public function human_turn( $x, $y )
    {
        // Get metadata
        $meta_data = $this->config->item('algorithm', 'match_four');
        
        if( $this->is_occupied($x, $y))
        {
            $this->remove_chip($x, $y);
        }
        else
        {
            //insert player chip
            $this->add_chip( $x, $y, $meta_data['values']['player'] );
        }
        $this->set_status( new Session_Status( $this->get_field(), $meta_data['values']['computer'], BOARD_STATUS::DEFAULT_STATUS ));
        
        return $this->get_status();
    }
    
    public function ai_turn()
    { 
        $meta_data = $this->config->item('algorithm', 'match_four');
        
        $ai_result = $this->run_algorithm();

        if( $ai_result->is_valid() && $ai_result->status != BOARD_STATUS::PLAYER1_WIN_STATUS )
            $this->add_chip( $ai_result->x, $ai_result->y, $meta_data['values']['computer'] );
        
        $this->set_status( new Session_Status( $this->get_field(), $ai_result->next, $ai_result->status, $ai_result->winning_chips ) );
        return $this->get_status();
    }


    public function __call( $method, $arguments )
    {
        if (!method_exists( $this->match_four_model, $method) )
        {
                throw new Exception('Undefined method lib_match_four::' . $method . '() called');
        }

        return call_user_func_array( array($this->match_four_model, $method), $arguments);
    }
    
    /**
    * __get
    *
    * Enables the use of CI super-global without having to define an extra variable.
    *
    * Taken from Ion_auth library
    * "I can't remember where I first saw this, so thank you if you are the original author. -Militis"
    *
    * @access	public
    * @param	$var
    * @return	mixed
    */
   public function __get($var)
   {
           return get_instance()->$var;
   }
    
}

