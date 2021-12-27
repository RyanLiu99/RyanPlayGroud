$("#btnSubmit").click(function () {
    let inputTxt = $("#inputTxt").val();
    console.log("send " + inputTxt)
    $.get("https://localhost:252?q=" + inputTxt,
        null, function (data, txtStatus) {
            $("#result").append(data + "<br/>");
        });
});