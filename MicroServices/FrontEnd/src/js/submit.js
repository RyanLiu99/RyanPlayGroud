$("#btnSubmit").click(function () {
    let inputTxt = $("#inputTxt").val();
    console.log("send " + inputTxt)
    $.get(
        {
            url:"https://localhost:352?q=" + inputTxt,
            
            success:  function (data, txtStatus) {
                $("#result").append(data + "<br/>");
            },

            error: function(xhr, txtStatus, error){  
                console.error(error)
            },


            complete: function(xhr, textStatus){
                console.log('Completed with status ' + textStatus);
            },


            dataType: "text",

            headers: {
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Headers': '*'
             }        
        });
    });