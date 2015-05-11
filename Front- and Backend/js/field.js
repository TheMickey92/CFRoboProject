/**
* Method to make the preview Tile visible
*
* @method previewTileIn
* @param {Integer} id: ID of the previewTile
*/
function previewTileIn(id) 
{
	document.getElementById(id).style.visibility='visible';
}

/**
* Method to hide a preview Tile
*
* @method previewTileOut
* @param {Integer} id: ID of the previewTile
*/
function previewTileOut(id)
{
    document.getElementById(id).style.visibility='hidden';
}

/**
* Method to print previewRow
*
* @method printPreviewRow
* @param {Integer} player: number of the current player
*/
function printPreviewRow(player, finished)
{   
	if(finished){
		$('#previewRow').html("<div class='row'><div class='cell previewCell'></div><div class='cell previewCell'></div><div class='cell previewCell'></div><div class='cell previewCell'></div><div class='cell previewCell'></div><div class='cell previewCell'></div><div class='cell previewCell'></div></div>");
	}
	else{
		var previewTile="";
		if(player==1)
		{
			previewTile="redTile";
		}
		else if(player==2)
		{
			previewTile="yellowTile";
		}
		var previewRow="<div class='row'>";
		for(var i=1; i<8; i++)
		{
			previewRow += "<div class='cell previewCell' onMouseOver=\"previewTileIn('previewTile"+i+"');\" onMouseOut=\"previewTileOut('previewTile"+i+"');\"><div class=\"circle previewTile "+previewTile+"\" id=\"previewTile"+i+"\"></div></div>";
		}
		previewRow += "</div>";
		$('#previewRow').html(previewRow);
	}
}

/**
* Method to draw the Field
*
* @method refreshField
* @param {Integer[][]} board: board with current information
* @param {Boolean} cheating: Is cheating enabled
*/
function refreshField(board,cheating, locked, winning_chips)
{
	var field="";
	for(var y = 0; y < 6; y++)
	{
		field += "<div class='row'>";
		for(var x = 0; x < 7; x++)
		{
			var isWinningChip=false;
			if (winning_chips!=null && winning_chips!=""){
				isWinningChip=getIsWinningChip(winning_chips, x,y);
			}
			else{
				isWinningChip=false;
			}
			var preX = x+1;		
			var circleClass="circle";
			if(board!="undefined")
			{
				if(board[x][y]==1)
				{
					if(cheating==false){
						circleClass="'circle redTile'";
					}
					else if(locked==true){
						circleClass="'circle redTile'"
					}
					else{
						circleClass="'circle redTile greenBorder' onClick='performMove("+x+","+y+");'";
					}
					if(isWinningChip==true){
						circleClass="'circle redTile winningChip'"
					}
				}
				else if(board[x][y]==2)
				{
					if(cheating==false){
						circleClass="'circle yellowTile'";
					}
					else if(locked==true){
						circleClass="'circle yellowTile'"
					}
					else{
						circleClass="'circle yellowTile greenBorder' onClick='performMove("+x+","+y+");'";
					}
					if(isWinningChip==true){
						circleClass="'circle yellowTile winningChip'"
					}
				}
				else if(locked==true)
				{
					circleClass="'circle finished'";
				}
				else
				{
					circleClass="'circle' onClick='performMove("+x+","+y+");'";
				}
			}			
			else
			{
				circleClass="'circle' onClick='performMove("+x+","+y+");'";
			}
			
			if(locked==false){
				field+="<div class='cell' onMouseOver=\"previewTileIn('previewTile"+preX+"');\" onMouseOut=\"previewTileOut('previewTile"+preX+"');\"><div class="+circleClass+" ></div></div>";
			}
			else{
				field+="<div class='cell' \"><div class="+circleClass+" ></div></div>";
			}
		}
		field += "</div>";	
	}
	
	$('#field').html(field);
}
function getIsWinningChip(winning_chips, x,y){
	if(winning_chips[0].x==x){
		if(winning_chips[0].y==y){
			return true;
		}
	}
	if(winning_chips[1].x==x){
		if(winning_chips[1].y==y){
			return true;
		}
	}
	if(winning_chips[2].x==x){
		if(winning_chips[2].y==y){
			return true;
		}
	}
	if(winning_chips[3].x==x){
		if(winning_chips[3].y==y){
			return true;
		}
	}
	return false;
}