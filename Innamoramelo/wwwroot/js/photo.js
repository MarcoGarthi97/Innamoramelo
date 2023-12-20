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

            photoObj[(imageId.substring(5) - 1)] = file 

            if (file) {
                const reader = new FileReader();

                reader.onload = function (e) {
                    image.src = e.target.result;
                };

                reader.readAsDataURL(file);

                var data = new FormData();
                data.append('file', file);
            }
        });
    }

    GetPhotos()
    function GetPhotos() {
        $.ajax({
            url: urlGetPhotos,
            type: "GET",
            success: function (result) {
                result.forEach(function (item, i) {
                    $('#image' + (i + 1)).attr('src', `data:image/png;base64,${item.bytes}`);

                    //const fileObject = new File([item.bytes], item.name); 
                    //photoObj.push(fileObject); 
                    
                    const bytes = atob(item.bytes);
                    const byteNumbers = new Array(bytes.length);
                    for (let i = 0; i < bytes.length; i++) {
                        byteNumbers[i] = bytes.charCodeAt(i);
                    }
                    const byteArray = new Uint8Array(byteNumbers);
                    const blob = new Blob([byteArray], { type: item.extension });
                    var file = new File([blob], item.name, { type: item.extension });

                    photoObj.push(file); 
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

            photoObj.forEach(function (file) {
                data.append("files", file);
            })

            $.ajax({
                url: urlInsertPhoto,
                type: "POST",
                data: data,
                processData: false,
                contentType: false,
                success: function (result) {
                    alert("Saved") 
                },
                error: function (error) {
                    console.log(error)
                }
            })
        }
    })
}) 