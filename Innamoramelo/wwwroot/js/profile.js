$(document).ready(function () {
    const sexualHTML = `<div class="row">
            <div class="col">
                <div class="mb-3"> 
                    <label for="selectGender" class="form-label">Gender</label>
                    <select class="form-select" id="selectGender">
                        <option value="" selected></option>
                        <option value="Male">Male</option>
                        <option value="Famale">Famale</option>
                        <option value="Other">Other</option>
                    </select>
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label for="selectSexualOrientation" class="form-label">Sexual orientation</label>
                    <select class="form-select" id="selectSexualOrientation">
                        <option value="" selected></option>
                        <option value="Heterosexual">Heterosexual</option>
                        <option value="Bisexual">Bisexual</option>
                        <option value="Homosexual">Homosexual</option>
                        <option value="Pansexual">Pansexual</option>
                    </select>
                </div>
            </div>
            <div class="col">
                <div class="mb-3">
                    <label for="selectLookingFor" class="form-label">What you are looking for?</label>
                    <select class="form-select" multiple  id="selectLookingFor">
                        <option value="" selected></option>
                        <option value="Man">Man</option>
                        <option value="Woman">Woman</option>
                        <option value="Other">Other</option>
                    </select>
                </div>
            </div>
        </div>`

    const jobHTML = `<div class="col">
        <div class="mb-3">
            <label for="inputEducation" class="form-label">Education</label>
            <input type="text" class="form-control" id="inputEducation">
        </div>
    </div>
    <div class="col">
        <div class="container">
            <div style='margin-bottom: 2%; color:"white"'>Job</div>
            <input class="form-control" id="inputJobFilter" type="text" placeholder="Search..">
            <ul class="list-group" id="listJobs">
            </ul>
        </div>
    </div>`

    const interestHTML = `<div class="container">
    <h4 style="color: white;">Select up to 5 of your passions</h4>
    <div class="row">
        <div class="col-md-4">
            <div class="interest" data-id="Travel">Travel</div>
            <div class="interest" data-id="Reading">Reading</div>
            <div class="interest" data-id="Music">Music</div>
            <div class="interest" data-id="Sports">Sports</div>
        </div>
        <div class="col-md-4">
            <div class="interest" data-id="Food">Food</div>
            <div class="interest" data-id="Art">Art</div>
            <div class="interest" data-id="Technology">Technology</div>
            <div class="interest" data-id="Movies">Movies</div>
        </div>
        <div class="col-md-4">
            <div class="interest" data-id="Photography">Photography</div>
            <div class="interest" data-id="Fitness">Fitness</div>
            <div class="interest" data-id="Gaming">Gaming</div>
            <div class="interest" data-id="Fashion">Fashion</div>
        </div>
    </div>
</div>`

    const photosHTML = `<div class="container">
    <h4 style="color: white;">Upload your photos</h4>
    <div class="row">
        <div class="col-md-4">
            <div class="card">
                <img src="https://via.placeholder.com/150" id="image1" class="card-img-top" alt="Immagine 1">
                <div class="card-body">
                    <label for="fileInput1" class="btn btn-primary">
                        Carica Foto
                        <input type="file" id="fileInput1" class="form-control-file d-none" accept="image/*">
                    </label>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card">
                <img src="https://via.placeholder.com/150" id="image2" class="card-img-top" alt="Immagine 2">
                <div class="card-body">
                    <label for="fileInput2" class="btn btn-primary">
                        Carica Foto
                        <input type="file" id="fileInput2" class="form-control-file d-none">
                    </label>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card">
                <img src="https://via.placeholder.com/150" id="image3" class="card-img-top" alt="Immagine 3">
                <div class="card-body">
                    <label for="fileInput3" class="btn btn-primary">
                        Carica Foto
                        <input type="file" id="fileInput3" class="form-control-file d-none">
                    </label>
                </div>
            </div>
        </div>
    </div>
</div>`

    var obj = {}

    LoadElements()
    function LoadElements() {
        //$('#card-body').append(sexualHTML) //funziona
        //$('#card-body').append(jobHTML)
        //$('#card-body').append(interestHTML)
        //$('#card-body').append(photosHTML)
        /*sotto a foto ci va questo codice        
    handleFileInput('fileInput1', 'image1');
    handleFileInput('fileInput2', 'image2');
    handleFileInput('fileInput3', 'image3');
        */
    }

    $('#sexualContinue').on("click", function () {
        if ($('#selectGender').val() == "" || $('#selectSexualOrientation').val() == "" || $('#selectLookingFor').val() == "") {
            alert("Values all fields")
            return;
        }

        obj.Gender = $('#selectGender').val()
        obj.SexualOrientation = $('#selectSexualOrientation').val()
        obj.LookingFor = $('#selectLookingFor').val()

        $('#card-body').empty()

    })

    $("#inputJobFilter").on('input', function () {
        var input = $("#inputJobFilter").val()

        if (input.length > 3)
            GetJobs(input)
    })

    function GetJobs(filter) {
        $.ajax({
            url: urlGetJobs,
            type: "POST",
            data: { filter: filter },
            success: function (result) {
                $('#listJobs').empty()

                var itemsHTML = ""

                result.forEach(function (item, i) {
                    itemsHTML += '<li class="list-group-item" id="item_' + i + '">' + item.name + '</li>'
                })

                $('#listJobs').append(itemsHTML)
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    $(document).on("click", ".list-group-item", function (e) {
        $('.list-group-item').removeClass('active')
        $('#' + e.target.id).addClass('active')
    })

    $("#selectLookingFor").on("click", "option", function (e) {
        //console.log($('#selectLookingFor').val())
    });

    $(".interest").click(function () {
        if (GetInterests().length < 5) {
            $(this).toggleClass("bg-primary text-white");
        }
        else {
            $(this).removeClass("bg-primary text-white");
        }

        GetInterests()
    });

    function GetInterests() {
        var selectedInterests = $(".interest.bg-primary").map(function () {
            return $(this).attr("data-id");
        }).get();

        return selectedInterests
    };

    function handleFileInput(inputId, imageId) {
        const input = document.getElementById(inputId);
        const image = document.getElementById(imageId);

        input.addEventListener('change', function () {
            const file = input.files[0];
            console.log(file)

            if (file) {
                const reader = new FileReader();

                reader.onload = function (e) {
                    image.src = e.target.result;
                };

                reader.readAsDataURL(file);
            }
        });
    }

    $("#inputMunicipalityFilter").on('input', function () {
        var input = $("#inputMunicipalityFilter").val()

        if (input.length >= 1)
            GetMunicipalities(input)
        else
            $('#listMunicipalities').empty()
    })

    function GetMunicipalities(filter) {
        $.ajax({
            url: urlGetMunicipalities,
            type: "POST",
            data: { filter: filter },
            success: function (result) {
                $('#listMunicipalities').empty()

                var itemsHTML = ""

                result.forEach(function (item, i) {
                    itemsHTML += '<li class="list-group-item" id="item_' + i + '">' + item.name + '</li>'
                })

                $('#listMunicipalities').append(itemsHTML)
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    $('#range').on('input', function(){
        $('#labelRange').html('Range: ' + $('#range').val() + "km") 
    })
})