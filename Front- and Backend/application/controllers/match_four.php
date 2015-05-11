<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

class Match_four extends CI_Controller {
 
    public function __construct()
    {
        parent::__construct();
        
        $this->load->library('lib_match_four');
    }
    
    public function turn( $x, $y )
    {
        echo $this->lib_match_four->human_turn( $x, $y );        
    }
    
    public function ai_turn( )
    {
        echo $this->lib_match_four->ai_turn();
    }
    
    public function startup()
    {
        echo $this->lib_match_four->startup();
    }

    public function robot_turn( $x )
    {
        echo $this->lib_match_four->robot_turn( $x );
    }

    public function field()
    {
        echo $this->lib_match_four->get_status();
        //echo json_encode($this->lib_match_four->get_field());
    }
    
    public function reset()
    {
        $this->lib_match_four->reset_field();
    }
    
}