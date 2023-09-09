$(document).ready(function () {
    $('#sendMail').on('click', function () {
        $.ajax({
            url: urlSendMail,
            type: "POST",
            success: function (result) {
                console.log(result)
            },
            error: function (error) {
                console.log(error)
            }
        })
    })

    $('.pin-input').on('input', function () {
        var $this = $(this);
        if ($this.val().length === 1) {
            $this.next('.pin-input').focus();
        }
    });

    $('#register').on('click', function () {
        var name = $('#name').val()
        var birthday = $('#birthday').val()
        var mail = $('#mail').val()
        var password = $('#password').val()

        if (name != '' && birthday != '' && mail != '' && password != '') {
            var obj = {}
            obj.Name = name
            obj.Birthday = birthday
            obj.Email = mail
            obj.Password = password

            var json = JSON.stringify(obj)

            $.ajax({
                url: urlRegister,
                type: "POST",
                data: { json: json },
                success: function (result) {
                    $('#pinModal').modal('show')
                },
                error: function (error) {
                    console.log(error)
                }
            })
        }
    })

    $('#verify').on('click', function () {
        var enteredPin = '';
        $('.pin-input').each(function () {
            enteredPin += $(this).val();
        });

        var obj = {}
        obj.Code = enteredPin
        obj.Created = new Date().toJSON()

        var json = JSON.stringify(obj)

        $.ajax({
            url: urlVerify,
            type: "POST",
            data: { json: json },
            success: function (result) {
                console.log(result)
            },
            error: function (error) {
                console.log(error)
            }
        })
    });
})