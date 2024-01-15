$(document).ready(function () {
    handleFileInput('fileInput1', 'image1');
    handleFileInput('fileInput2', 'image2');
    handleFileInput('fileInput3', 'image3');

    var photoUpdate = [];

    function handleFileInput(inputId, imageId) {
        const input = document.getElementById(inputId);
        const image = document.getElementById(imageId);

        input.addEventListener('change', function () {
            const file = input.files[0];

            update = {
                position: imageId.substring(5) - 2,
                name: file.name,
                type: file.type,
                size: file.size,
            };

            if (file) {
                const reader = new FileReader();

                reader.onload = function (e) {
                    image.src = e.target.result;

                    var base64Part = e.target.result.split(',')[1];

                    // Decodifica la parte base64
                    var binaryString = atob(base64Part);

                    // Crea un array di byte Uint8Array
                    var uint8Array = new Uint8Array(binaryString.length);

                    // Riempie l'array con i byte dalla stringa decodificata
                    for (var i = 0; i < binaryString.length; i++) {
                        uint8Array[i] = binaryString.charCodeAt(i);
                    }

                    console.log(uint8Array)

                    update.content = uint8Array;
                    
                    photoUpdate.push(update);

                    console.log(photoUpdate)
                };

                reader.readAsDataURL(file);
            }
        });
    }

    GetPhotos();

    function GetPhotos() {
        $.ajax({
            url: urlGetPhotos,
            type: "GET",
            success: function (result) {
                console.log(result);
                result.forEach(function (item, i) {
                    $('#image' + (i + 1)).attr('src', item.url);
                });
            },
            error: function (error) {
                console.log(error);
            }
        });
    }

    $('#btnSave').on('click', function () {
        var json = JSON.stringify(photoUpdate);
        console.log(json);

        if (photoUpdate.length > 0) {
            $.ajax({
                url: urlInsertPhoto,
                type: "POST",
                contentType: 'application/json',
                data: json,
                success: function (result) {
                    alert("Saved");
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    });
});