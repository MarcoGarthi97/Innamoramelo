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
                        <option value="Male">Male</option>
                        <option value="Famale">Famale</option>
                        <option value="Other">Other</option>
                    </select>
                </div>
            </div>
        </div>
        <button type="button" id="back" class="btn btn-primary">←</button>
        <div class="float-end">
            <button type="button" id="sexualContinue" class="btn btn-primary">Continue</button>
        </div>`

    var obj = {}

    LoadElements()
    function LoadElements() {
        //$('#card-body').append(sexualHTML) 
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

    GetJobs()
    function GetJobs(){
        const options = { method: 'GET', headers: { accept: 'application/json' } };

        fetch('https://apis.druva.com/insync/legalholds/v4/jobs', options)
            .then(response => response.json())
            .then(response => console.log(response))
            .catch(err => console.error(err));
    }

    $("#selectLookingFor").on("click", "option", function (e) {
        //console.log($('#selectLookingFor').val())
    });
})