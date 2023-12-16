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
            <div class="interest" data-id="Travel" id="Travel">Travel</div>
            <div class="interest" data-id="Reading" id="Reading">Reading</div>
            <div class="interest" data-id="Music" id="Music">Music</div>
            <div class="interest" data-id="Sports" id="Sports">Sports</div>
        </div>
        <div class="col-md-4">
            <div class="interest" data-id="Food" id="Food">Food</div>
            <div class="interest" data-id="Art" id="Art">Art</div>
            <div class="interest" data-id="Technology" id="Technology">Technology</div>
            <div class="interest" data-id="Movies" id="Movies">Movies</div>
        </div>
        <div class="col-md-4">
            <div class="interest" data-id="Photography" id="Photography">Photography</div>
            <div class="interest" data-id="Fitness" id="Fitness">Fitness</div>
            <div class="interest" data-id="Gaming" id="Gaming">Gaming</div>
            <div class="interest" data-id="Fashion" id="Fashion">Fashion</div>
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

    const cityHTML = `<div class="row">
<div class="col">
    <div class="container">
        <div style='margin-bottom: 2%; color:"white"'>Municipality</div>
        <input class="form-control" id="inputMunicipalityFilter" type="text" placeholder="Search..">
        <ul class="list-group" id="listMunicipalities">
        </ul>
    </div>
</div>
<div class="col">
    <div class="mb-3">
        <label for="range" class="form-label" id="labelRange">Range: 5km</label>
        <input type="range" class="form-range" id="range" min="5" max="100" value="5">
    </div>
</div>
</div>
<div class="row">
<div class="col">
    <div class="mb-3">
        <label for="inputBio" class="form-label">Biography</label>
        <textarea class="form-control" aria-label="With textarea" id="inputBio"></textarea>
    </div>
</div>
</div>`

    var obj = {}
    var page = 1

    var listCity = []

    LoadElements()
    function LoadElements() {
        if (page == 1)
            $('#card-body').append(sexualHTML)
        else if (page == 2)
            $('#card-body').append(jobHTML)
        else if (page == 3)
            $('#card-body').append(interestHTML)
        else if (page == 4)
            $('#card-body').append(cityHTML)
    }

    $('#btnContinue').on("click", function () {
        if (page == 1) {
            if ($('#selectGender').val() == "" || $('#selectSexualOrientation').val() == "" || $('#selectLookingFor').val() == "") {
                alert("Values all fields")
                return;
            }

            obj.Gender = $('#selectGender').val()
            obj.SexualOrientation = $('#selectSexualOrientation').val()
            obj.LookingFor = $('#selectLookingFor').val()
        }
        else if (page == 2) {
            if ($('#inputEducation').val() == "" || $('#inputJobFilter').val() == "") {
                alert("Values all fields")
                return;
            }

            obj.Education = $('#inputEducation').val()
            obj.Job = $('#inputJobFilter').val()
        }
        else if (page == 3) {
            var interests = GetInterests()

            if (interests.length < 3) {
                alert("Select at least 3 interests")
                return
            }

            obj.Passion = interests
        }
        else if (page == 4) {
            if ($('#inputMunicipalityFilter').val() == "" || $('#range').val() == "" || $('#inputBio').val() == "") {
                alert("Values all fields")
                return;
            }
            else if (listCity.find(x => x.name == $('#inputMunicipalityFilter').val()).length < 0) {
                alert("Choose your city")
                return;
            }

            obj.Location.Id = listCity[0].id
            obj.Location.Name = listCity[0].name
            obj.RangeKm = $('#range').val()
            obj.Bio = $('#inputBio').val()
        }

        if (page < 5) {
            $('#card-body').empty()

            page++
            LoadElements()

            console.log(obj)
        }
    })

    $('#btnBack').on("click", function () {
        if (page > 0) {
            $('#card-body').empty()

            page--
            LoadElements()

            console.log(obj)
        }

        if (page == 1) {
            $('#selectGender').val(obj.Gender)
            $('#selectSexualOrientation').val(obj.SexualOrientation)
            $('#selectLookingFor').val(obj.LookingFor)
        }
        else if (page == 2) {
            $('#inputEducation').val(obj.Education)
            $('#inputJobFilter').val(obj.Job)

            GetJobs(obj.Job)
        }
        else if (page == 3) {
            obj.Passion.forEach(function (item) {
                $('#' + item).addClass("bg-primary")
            })
        }
        else if (page == 4) {
            $('#inputMunicipalityFilter').val(obj.Location.Name)
            $('#range').val(obj.RangeKm)
            $('#inputBio').val(obj.Bio)

            GetMunicipalities()
        }
    })

    $(document).on('input', "#inputJobFilter", function () {
        var input = $("#inputJobFilter").val()

        if (input.length > 2)
            GetJobs(input)
    })

    function GetJobs(filter) {
        $.ajax({
            url: urlGetJobs,
            type: "POST",
            data: { filter: filter },
            success: function (result) {
                console.log(result)
                $('#listJobs').empty()

                var itemsHTML = ""

                result.forEach(function (item, i) {
                    itemsHTML += '<li class="list-group-item job-item" id="item_' + i + '">' + item.name + '</li>'
                })

                $('#listJobs').append(itemsHTML)
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    $(document).on('click', '.job-item', function () {
        var item = $(this)
        var val = $('#' + item[0].id).html()

        $("#inputJobFilter").val(val)

        GetJobs(val)
    })

    $(document).on("click", ".list-group-item", function (e) {
        $('.list-group-item').removeClass('active')
        $('#' + e.target.id).addClass('active')
    })

    $("#selectLookingFor").on("click", "option", function (e) {
        //console.log($('#selectLookingFor').val())
    });

    $(document).on("click", ".interest", function () {
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

    var timerID = null;
    function stopTimer() {
        if (timerID) {
            clearTimeout(timerID);
            timerID = null;
        }
    }

    $(document).on('input', "#inputMunicipalityFilter", function () { 
        var input = $("#inputMunicipalityFilter").val()

        //const timer1 = setTimeout(GetMunicipalities(input), 2000); 

        if (input.length > 2){
            stopTimer();
            timerID = setTimeout(GetMunicipalities, 2000);
        }
        else
            $('#listMunicipalities').empty()
    })

    $(document).on('click', '.city-item', function () {
        var item = $(this)
        var val = $('#' + item[0].id).html()

        $("#inputMunicipalityFilter").val(val)

        GetMunicipalities()
    })

    function GetMunicipalities() { 
        var filter = $("#inputMunicipalityFilter").val()

        $.ajax({
            url: urlGetMunicipalities,
            type: "POST",
            data: { filter: filter },
            success: function (result) {
                $('#listMunicipalities').empty()

                var itemsHTML = ""

                result.forEach(function (item) {
                    itemsHTML += '<li class="list-group-item city-item" id="item_' + item.id + '">' + item.name + '</li>'
                })

                $('#listMunicipalities').append(itemsHTML)
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    $('#range').on('input', function () {
        $('#labelRange').html('Range: ' + $('#range').val() + "km")
    })
})