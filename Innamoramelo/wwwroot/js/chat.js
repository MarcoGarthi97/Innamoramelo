$(document).ready(function () {
    LoadComponents()
    function LoadComponents() {
        //GetContacts()
        GetMatches()
    }

    function GetMatches() {
        $.ajax({
            url: urlGetMatches,
            type: "POST",
            success: function (result) {
                var listId = []
                result.forEach(function (item) {
                    listId.push(item.usersId[0])
                }) 

                GetContacts(listId)
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    function GetContacts(listId) {
        var json = JSON.stringify(listId)

        $.ajax({
            url: urlGetContacts,
            type: "POST",
            data: { json: json },
            success: function (result) {
                console.log(result)
                $('#div-contacts').empty()

                result.forEach(function (item) {
                    var msg = ""
                    if(item.isReceiverMessage)
                        msg += item.receiverName + ": "
                    else
                        msg += "You: "

                    msg += item.content.substring(0, 15)                    

                    var htmlContant = `<div id="` + item.id + `" class="conversation-list"><a href="#" class="list-group-item list-group-item-action border-0 contact-item-list">`
                    
                    if(item.undisplayedMessages > 0){
                        if(item.undisplayedMessages < 10)                            
                            htmlContant += `<div class="badge bg-success float-end contanct-badge">` + item.undisplayedMessages + `</div>`
                        else                            
                            htmlContant += `<div class="badge bg-success float-end contanct-badge">+9</div>`
                    }

                    htmlContant += `<div class="d-flex align-items-start">
                        <img src="https://bootdey.com/img/Content/avatar/avatar5.png" class="rounded-circle me-1"
                             width="40" height="40">
                        <div class="flex-grow-1 ms-3">
                        ` + item.receiverName +
                        `<div class="small"><span class="fas fa-circle text-success me-1"></span>` + msg +`</div>
                        </div>
                    </div>
                    </a></div>`

                    $('#div-contacts').append(htmlContant) 
                }) 
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    $(document).on('click', '.conversation-list', function(e){
        console.log(e.currentTarget.id) 
        VisualizeConversation(e.currentTarget.id)
    })  

    function GetConversation(receiverId, skip, limit) {
        var obj = {}
        obj.receiverId = receiverId

        if (skip != null)
            obj.skip = skip

        if (limit != null)
            obj.limit = limit

        var json = JSON.stringify(obj)

        $.ajax({
            url: urlGetConversation,
            type: "POST",
            data: { json: json },
            success: function (result) {
                console.log(result)
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    function VisualizeConversation(receiverId){
        $.ajax({
            url: urlVisualizeMessages,
            type: "POST",
            data: { receiverId: receiverId },
            success: function (result) {
                console.log(result)
                GetBadgeNewMessage(receiverId)
            },
            error: function (error) {
                console.log(error)
            }
        }) 
    }

    function GetBadgeNewMessage(receiverId){
        $.ajax({
            url: urlGetContact,
            type: "POST",
            data: { receiverId: receiverId },
            success: function (result) {
                console.log(result) 
                $("div#" + receiverId + " .contact-item-list .contanct-badge").remove()

                if(result.undisplayedMessages > 0){  
                    var htmlContant = ''

                    if(result.undisplayedMessages < 10)                            
                        htmlContant = `<div class="badge bg-success float-end contanct-badge">` + result.undisplayedMessages + `</div>`
                    else                            
                        htmlContant = `<div class="badge bg-success float-end contanct-badge">+9</div>`

                    $('#' + receiverId + ' .contact-item-list').prepend(htmlContant) 
                }
            }, 
            error: function (error) {
                console.log(error)
            }
        }) 
    }

    $('#test').on('click', function(){
        GetBadgeNewMessage('655bb75d05c9f13660284221') 
    })
 
    Test()
    function Test(){
        var val = $('.chat-message-right').height()
        val += 5

        //$('.chat-message-right').height(val)
    }
})