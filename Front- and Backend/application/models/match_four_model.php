<?php  if ( ! defined('BASEPATH')) exit('No direct script access allowed');

/**
 * @class match_four_model
 *
 * @author Thorben
 */
class match_four_model extends CI_Model
{
    private $x;
    
    private $y;
    
    private $value;
    
    public function __construct()
    {
        parent::__construct();
        
        $this->load->library('session');
        $this->load->config('match_four', TRUE);
        
        if( !$this->session->userdata('current_status') )
        {
            $this->reset_field();
        }
            
    }
    
    public function x( $value )
    {
        $field_dimension = $this->config->item('field_size', 'match_four');
        if( $value < 0 || $value >= $field_dimension['x'] )
            throw new Exception( "Invalid cell position x: $value" );
        
        $this->x = $value;
        
        return $this;
    }
    
    public function y( $value )
    {
        $field_dimension = $this->config->item('field_size', 'match_four');
        if( $value < 0 || $value >= $field_dimension['y'] )
            throw new Exception( "Invalid cell position y: $value" );
        
        if( $this->x === null)
            throw new Exception( "X must be set" );
        
        $this->y = $value;
        
        return $this;
    }
    
    public function value( $value )
    {
        $config = $this->config->item('algorithm', 'match_four');
        
        if( !in_array( $value, $config['values']) )
                throw new Exception( "Invalid cell value: $value" );
        
        $this->value = $value;
        
        return $this;
    }
    
    public function save()
    {
        if( !ctype_digit( $this->x ) || $this->x === null )
            throw new Exception( "Incorrect parameter X: ".$this->x );
        
        if( !ctype_digit( $this->y ) || $this->y === null )
            throw new Exception( "Incorrect parameter Y:".$this->y );
        
        if( empty( $this->value ) )
            throw new Exception( "Value is not set" );
        
        $field = $this->get_field();
        $this->set_last_field( $field );
        
        $field_meta = $this->config->item('field_size', 'match_four');
        $algorithm_config = $this->config->item('algorithm', 'match_four');
        
        while( $this->y < ($field_meta['y']-1) && $field[$this->x][$this->y+1]==$algorithm_config['values']['empty'])
            $this->y++;
        
        $field[$this->x][$this->y] = $this->value;
        $this->set_field($field);
        
        $this->x = $this->y = $this->value = null;
        
        return true;
        
    }

    public function is_occupied( $x, $y )
    {
        $config = $this->config->item('algorithm', 'match_four');
        
        $board = $this->get_field();
        return $board[$x][$y] != $config['values']['empty'];
    }
    
    public function get_field()
    {
        return $this->get_status()->board;
    }
    
    public function get_status()
    {
        return $this->session->userdata('current_status');
    }

    public function set_field( $field )
    {
        if ( $this->check_field_dimensions( $field ) )
        {
            $state = $this->get_status();
            $state->board = $field;
            $this->set_status($state);
        }
    }

    public function set_status( Session_Status $status )
    {
        $this->session->set_userdata( 'current_status', $status );
    }
    
    public function get_last_field()
    {
        return $this->session->userdata('last_field');
    }
    
    public function set_last_field( $field )
    {
        if ( $this->check_field_dimensions( $field ) )
        {
            $this->session->set_userdata( 'last_field', $field );
        }
        else
        {
            throw new Exception( "Invalid Board Dimensions." );
        }
    }

    private function check_field_dimensions( $field )
    {
        $field_size = $this->config->item('field_size', 'match_four');
        
        if( !is_array( $field ) || count( $field ) != $field_size['x'] )
            return false;

        foreach( $field as $field_row )
        {
            if( count( $field_row ) != $field_size['y'] )
                return false;
        }   
        
        return true;
    }
    
    public function reset_field()
    {
        //get config
        $field_size = $this->config->item('field_size', 'match_four');
        $algorithm  = $this->config->item('algorithm', 'match_four');

        //initialize field
        $field = array();
        for( $x = 0; $x < $field_size['x']; $x++ )
        {
            for( $y = 0; $y < $field_size['y']; $y++ )
                $field[$x][$y] = $algorithm['values']['empty'];
        } 
        $state = new Session_Status( $field, 1, BOARD_STATUS::DEFAULT_STATUS);
        $this->set_status($state);
        $this->set_last_field($field);
    }
}

/*
 * Abstract class BOARD_STATUS
 * used as Enum for board Values
 */
abstract class BOARD_STATUS
{
    const ERROR_STATUS = -5;
    const UNDEFINED_ERROR = -4;
    const FORMAT_ERROR = -3;
    const CHEATED_STATUS = -2;
    const DEFAULT_STATUS = -1;
    const REMIS_STATE = 0;
    const PLAYER1_WIN_STATUS = 1;
    const PLAYER2_WIN_STATUS = 2;
}
//EOF match_four_model