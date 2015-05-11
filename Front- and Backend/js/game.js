/**
* Method to print field when the Website is loaded 
*
* @method document.ready
*/
$( document ).ready(function() 
{
	refreshCurrentField();	
});

/**
* Method to perform Move
* 1. Get JSON
* 2. Refresh Field & Info
*
* @method performMove
* @param {Integer} x: x-coordinate of the clicked tile
* @param {Integer} y: y-coordinate of the clicked tile
*/
function performMove(x,y)
{

	$.ajax({url:"index.php/match_four/turn/"+x+"/"+y,success:function(result){
		var arr = JSON.parse(result);
		refreshInfo(arr.player, arr.state);
		var cheatingEnabled=getCheatingEnabled();
		var isGameFinished=getGameFinished(arr.state);
		refreshField(arr.board, cheatingEnabled, true, null);
		printPreviewRow(arr.player, isGameFinished);
		
		
		
		spinner=loaderAnimationON('Spin');
		$.ajax({url:"index.php/match_four/turn/ai/",success:function(result){
			var arr = JSON.parse(result);
			refreshInfo(arr.player, arr.state);
			var cheatingEnabled=getCheatingEnabled();
			var isGameFinished=getGameFinished(arr.state);
			refreshField(arr.board, cheatingEnabled, isGameFinished, arr.winning_chips); 
			printPreviewRow(arr.player, isGameFinished);
			spinner.stop();
			//alert(arr.winning_chips[0].x);
		}});
			
	}});
	
}

function getGameFinished(state){
	if(state==-1){
		return false;
	}
	return true;
}

var spinner;

 function loaderAnimationON (div) {
        var opts = {
            lines: 13, // The number of lines to draw
            length: 20, // The length of each line
            width: 10, // The line thickness
            radius: 30, // The radius of the inner circle
            corners: 1, // Corner roundness (0..1)
            rotate: 0, // The rotation offset
            direction: 1, // 1: clockwise, -1: counterclockwise
            color: '#000', // #rgb or #rrggbb or array of colors
            speed: 1, // Rounds per second
            trail: 60, // Afterglow percentage
            shadow: false, // Whether to render a shadow
            hwaccel: false, // Whether to use hardware acceleration
            className: 'spinner', // The CSS class to assign to the spinner
            zIndex: 2e9, // The z-index (defaults to 2000000000)
            top: '50%', // Top position relative to parent in px
            left: '50%' // Left position relative to parent in px
        };
        var target = document.getElementById(div);
        var spinner = new Spinner(opts).spin(target);


        return spinner;
    };


/**
* Method to refresh the current field
*
* @method refreshCurrentField
*/
function refreshCurrentField(){
	$.ajax({url:"index.php/match_four/field/",success:function(result){
		var arr = JSON.parse(result);
		refreshField(arr.board,getCheatingEnabled(), getGameFinished(arr.state), arr.winning_chips);	
		printPreviewRow(arr.player, getGameFinished(arr.state));
		refreshInfo(arr.player, arr.state);
	}});
	
}

function reset(){
	$.ajax({url:"index.php/match_four/reset/",success:function(result){
		refreshCurrentField();
	}});
	printPreviewRow(1, false);
	refreshInfo(1,-1);
}

/**
* Method to get the current cheating status
*
* @method getCheatingStatus
*/
function getCheatingEnabled(){
	return $( "#switch1" ).prop('checked');
}

/**
* Method to refresh the info section
*
* @method performMove
* @param {Integer} player: number of the current player
* @param {Integer} state: id of the current state
*/
function refreshInfo(player, state)
{

	var color="";
	var currentState="";
	
	if(player==1){
		color="Rot";
	}
	else {
		color="Gelb";
	}
	
	if(state==1){
		currentState="<span class='redText'>Rot gewinnt</span>";
	}
	else if(state==2){
		currentState="<span class='yellowText'>Gelb gewinnt</span>";
	}
	else if(state==-1){
		currentState="Wartet auf nächsten Zug...";
	}
	else if(state==0){
		currentState="Unentschieden";
	}
	else if(state==-2){
		currentState="Geschummelt";
	}
	else{
		currentState="Error";
	}
	if(state==-1){
		$("#currentPlayer").html(color + " ist am Zug");
	}
	else{
		$("#currentPlayer").html("");
	}
	$("#currentState").html(currentState);
	
	if(state==1 || state==2 || state==0){
		$( "#currentState" ).fadeOut( "fast");
		$( "#currentState" ).fadeIn( "slow");
	}
	  
}