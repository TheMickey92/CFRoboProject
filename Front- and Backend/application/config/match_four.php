<?php  if ( ! defined('BASEPATH')) exit('No direct script access allowed');

//Field size
$config['field_size']['x'] = 7;
$config['field_size']['y'] = 6;

//general information about the algorithm
$config['algorithm']['location']                = "D:".DIRECTORY_SEPARATOR.'Programme'.DIRECTORY_SEPARATOR."XAMPP".DIRECTORY_SEPARATOR."htdocs".DIRECTORY_SEPARATOR."4Gewinnt".DIRECTORY_SEPARATOR."match_four".DIRECTORY_SEPARATOR."CurrentRelease".DIRECTORY_SEPARATOR;
$config['algorithm']['name']                    = "CFConsole";
$config['algorithm']['extension']               = ".exe";
$config['algorithm']['additional_parameters']   = "";
$config['algorithm']['parameter_structure']['logic'] = "logic {b} {a} {+}";
$config['algorithm']['parameter_structure']['bot']  = "robot {x} {ip} {+}";
$config['algorithm']['parameter_structure']['vision']  = "vision {+}";

//possible values
$config['algorithm']['values']['empty']     = 0;
$config['algorithm']['values']['player']    = 1;
$config['algorithm']['values']['computer']  = 2;

//metadata for parameter_structure
$config['algorithm']['parameter']['module']     = "{m}";
$config['algorithm']['parameter']['before']     = "{b}";
$config['algorithm']['parameter']['after']      = "{a}";
$config['algorithm']['parameter']['x-coordinate'] = "{x}";
$config['algorithm']['parameter']['ip']         = "{ip}";
$config['algorithm']['parameter']['additional'] = "{+}";

$config['hardware']['camera'] = 'HD 720P Webcam';

$config['opponent_url'] = 'http://172.16.54.98:65111/connect4/doTurn';