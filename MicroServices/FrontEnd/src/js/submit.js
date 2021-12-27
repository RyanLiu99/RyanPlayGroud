$( "#btnSubmit" ).click(function() {
    let inputTxt = $("#inputTxt").val();
    $("#result").append( inputTxt + "<br/>");
  });