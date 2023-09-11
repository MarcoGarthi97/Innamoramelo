$(document).ready(function(){
    $("#selectLookingFor").on("click", "option", function(e) {
        console.log($('#selectLookingFor').val())
    });
})