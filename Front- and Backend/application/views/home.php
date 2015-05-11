<div id="body">
	<div id="game" >
		<div id="previewRow"></div>
		<div id="field" style="float:left;"></div>
		<div id="info">
			<div><br />
				<span id="currentPlayer">Rot ist am Zug</span><br />
				<span id="currentState">Wartet auf Zug</span>
				
				<br /><br /><br /><br /><br /><br /><br />
				
				<div class="center">
					<input type="checkbox" id="switch1" name="switch1" class="switch"  />
					<label for="switch1">Cheating</label>
				</div>
				<br /><br />
				<button onClick="reset();">Reset</button>
			</div>
		</div>
	</div>
	<div id="Spin"></div>
	<script>
	// Refresh Field braucht das aktuelle Spielfeld
	$( "#switch1" ).click(function() {
		refreshCurrentField();
	//alert($( this ).prop('checked'));
});
	</script>
	  <script>
  $(function() {
    $( "input[type=submit], a, button" )
      .button()
      .click(function( event ) {
        event.preventDefault();
      });
  });
  </script>