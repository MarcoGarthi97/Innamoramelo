$(document).ready(function () {
    handleFileInput('fileInput1', 'image1');
    handleFileInput('fileInput2', 'image2');
    handleFileInput('fileInput3', 'image3');

    var photoObj = []

    function handleFileInput(inputId, imageId) {
        const input = document.getElementById(inputId);
        const image = document.getElementById(imageId);

        input.addEventListener('change', function () {
            const file = input.files[0];
            photoObj.push(file)

            if (file) {
                const reader = new FileReader();

                reader.onload = function (e) {
                    image.src = e.target.result;
                };

                reader.readAsDataURL(file);

                var data = new FormData();
                data.append('file', file);

                console.log(data)

            }
        });
    }

    GetPhotos()
    function GetPhotos(){
        $.ajax({
            url: urlGetPhotos,
            type: "GET",
            success: function (result) {
                console.log(result)
                result.forEach(function(item, i){
                    $('#image' + (i + 1)).attr('src', `data:image/png;base64,${item.bytes}`); 
                })                  
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    $('#btnSave').on('click', function () {
        if (photoObj.length > 0) {
            var data = new FormData();

            photoObj.forEach(function(file){
                data.append("files", file);
            })             

            $.ajax({
                url: urlInsertPhoto,
                type: "POST",
                data: data,
                processData: false,
                contentType: false,
                success: function (result) {
                    console.log(result)
                },
                error: function (error) {
                    console.log(error)
                }
            })
        }
    })
}) 