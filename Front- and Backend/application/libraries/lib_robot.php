<?php  if ( ! defined('BASEPATH')) exit('No direct script access allowed');

/**
 * @class lib_robot
 *
 * @author Thorben
 */
class lib_robot
{
    public function __construct()
    {
        $this->load->config('match_four', TRUE);
    }
    
    /**
     * @return Array List of view devices
     */
    public function get_device_list()
    {
        //get config
        $algorithm  = $this->config->item('algorithm', 'match_four');
        
        //search for algorithm
        $alg    = $algorithm['location'].$algorithm['name'].$algorithm['extension'];
        if( !file_exists( $alg ))
                throw new Exception( "Algorithm ".$algorithm['name'].$algorithm['extension']." not found in ".$algorithm['location'].". Did you forget a DIRECTORY_SEPARATOR?");
        
        //get parameter structure
        $param      = $algorithm['parameter_structure']['vision'];
        $parameters = $algorithm['parameter'];
        
        $param = str_replace($parameters['additional'], "devices", $param);
        
        //execute algorithm
        file_put_contents("commands.txt", date( "H:m:i - " ).$param."\r\n", FILE_APPEND | LOCK_EX);
        $result = exec( "$alg $param", $debug, $return_var );
        
        return explode( "; ", $result );
    }
    
    /**
     * 
     * @param int $index
     * @return string JSON representation of the viewed field
     * @throws Exception if Algorithm not found
     */
    public function get_view_of_device( $index )
    {
        //get config
        $algorithm  = $this->config->item('algorithm', 'match_four');
        
        //search for algorithm
        $alg    = $algorithm['location'].$algorithm['name'].$algorithm['extension'];
        if( !file_exists( $alg ))
                throw new Exception( "Algorithm ".$algorithm['name'].$algorithm['extension']." not found in ".$algorithm['location'].". Did you forget a DIRECTORY_SEPARATOR?");
        
        //get parameter structure
        $param      = $algorithm['parameter_structure']['vision'];
        $parameters = $algorithm['parameter'];
        
        $param = str_replace($parameters['additional'], $index, $param);
        
        //execute algorithm
        file_put_contents("commands.txt", date( "H:m:i - " ).$param."\r\n", FILE_APPEND | LOCK_EX);
        return exec( "$alg $param", $debug, $return_var );
        
    }
    
    public function insert_into( $column )
    {
        //get config
        $algorithm  = $this->config->item('algorithm', 'match_four');
        
        //search for algorithm
        $alg    = $algorithm['location'].$algorithm['name'].$algorithm['extension'];
        if( !file_exists( $alg ))
                throw new Exception( "Algorithm ".$algorithm['name'].$algorithm['extension']." not found in ".$algorithm['location'].". Did you forget a DIRECTORY_SEPARATOR?");
        
        //get parameter structure
        $param      = $algorithm['parameter_structure']['bot'];
        $parameters = $algorithm['parameter'];
        
        $param = str_replace($parameters['x-coordinate'], $column, $param);
        $param = str_replace($parameters['ip'], "", $param);
        $param = str_replace($parameters['additional'], "", $param);
        
        //execute algorithm
        file_put_contents("commands.txt", date( "H:m:i - " ).$param."\r\n", FILE_APPEND | LOCK_EX);
        $return_value = exec( "$alg $param", $debug, $return_var );
        
        switch ($return_value)
        {
            case "1":
                return true;
            case "-1":
                throw new Exception("Undefined Error while executing Robot throw-in");
            case "-2":
                throw new Exception("Robot not connected");
            case "-3":
                throw new Exception("Robot timed out");
            default:
                throw new Exception("Unknown return status");
        }
        
    }
    
    public function logic( $before, $after )
    {
        //get config
        $algorithm  = $this->config->item('algorithm', 'match_four');
        //search for algorithm
        $alg    = $algorithm['location'].$algorithm['name'].$algorithm['extension'];
        if( !file_exists( $alg ))
                throw new Exception( "Algorithm ".$algorithm['name'].$algorithm['extension']." not found in ".$algorithm['location'].". Did you forget a DIRECTORY_SEPARATOR?");
        
        //get parameter structure
        $param      = $algorithm['parameter_structure']['logic'];
        $parameters = $algorithm['parameter'];

        //replace structure with parameters
        $before = json_encode( $before );
        $after  = json_encode( $after );
        
        $param = str_replace($parameters['before'], $before, $param);
        $param = str_replace($parameters['after'],  $after, $param);
        $param = str_replace($parameters['additional'], $algorithm['additional_parameters'], $param);
        
        //execute algorithm
        file_put_contents("commands.txt", date( "H:m:i - " ).$param."\r\n", FILE_APPEND | LOCK_EX);
        $result = exec( "$alg $param", $debug, $return_var );
        
        return new AI_Result($result);
    }
    
    public function send_to_opponent( $result )
    {
        $url  = $this->config->item('opponent_url', 'match_four');

        $url.="?col=$result";
     
        header("Location:$url");
    }

}

class AI_Result
{   
    public $x;
    
    public $y;
    
    public $next;
    
    public $status;
    
    public $winning_chips;
    
    public function __construct( $serialized )
    {   
        //split string
        $values = explode( " ", $serialized );
        
        if( count( $values ) != 4 &&  count( $values ) != 12 )
        {
            $this->status = BOARD_STATUS::ERROR_STATUS;
        }
        else
        {
            $this->x = $values[0];
            $this->y = $values[1];
            $this->next = $values[2];
            $this->status = $values[3]; 
            $this->winning_chips = array();
            
            if( count( $values ) == 12 ) // Game has finished - append winning chips
            {
                $this->winning_chips[0]['x'] = $values[4];
                $this->winning_chips[0]['y'] = $values[5];
                $this->winning_chips[1]['x'] = $values[6];
                $this->winning_chips[1]['y'] = $values[7];
                $this->winning_chips[2]['x'] = $values[8];
                $this->winning_chips[2]['y'] = $values[9];
                $this->winning_chips[3]['x'] = $values[10];
                $this->winning_chips[3]['y'] = $values[11];
            }
        }
    }
    
    public function is_valid() //return false if state is cheated or error
    {
        return $this->status != BOARD_STATUS::CHEATED_STATUS && $this->status != BOARD_STATUS::ERROR_STATUS;
    }
    
    public function has_finished()
    {
        return $this->somebody_won() || $this->status == BOARD_STATUS::REMIS_STATE;
    }
    
    public function somebody_won()
    {
        return $this->status == BOARD_STATUS::PLAYER1_WIN_STATUS 
                || $this->status == BOARD_STATUS::PLAYER2_WIN_STATUS;
    }


    public function __toString()
    {
        return json_encode( $this );
    }

}

class Session_Status
{
    public $board;
    
    public $player;
    
    public $state;
    
    public $winning_chips;
    
    public function __construct( $board, $player, $state, $winning_chips = null )
    {
        $this->board    = $board;
        $this->player   = $player;
        $this->state    = BOARD_STATUS::ERROR_STATUS;
        $this->winning_chips = $winning_chips;
        
        if ( $state == BOARD_STATUS::CHEATED_STATUS
                || $state == BOARD_STATUS::DEFAULT_STATUS
                || $state == BOARD_STATUS::ERROR_STATUS
                || $state == BOARD_STATUS::REMIS_STATE
                || $state == BOARD_STATUS::PLAYER1_WIN_STATUS
                || $state == BOARD_STATUS::PLAYER2_WIN_STATUS 
        )
        {
            $this->state    = $state;
        }            
    }
 
    public function __toString()
    {
        return json_encode( $this );
    }
}
//EOF lib_robot