$(document).ready(function () {
    var _receiverId = ''
    var _contancts = {}

    LoadComponents()

    function LoadComponents() {
        GetMatches()
    }

    function GetMatches() {
        $.ajax({
            url: urlGetMatches,
            type: "POST",
            success: function (result) {
                var listId = []
                result.forEach(function (item) {
                    listId.push(item)
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
                _contancts = result

                CreateContacts(_contancts)
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    function CreateContacts(result) {
        $('#div-contacts').empty()

        result.forEach(function (item) {
            var msg = ""
            if (item.isReceiverMessage)
                msg += item.receiverName + ": "
            else
                msg += "You: "

            msg += item.content.substring(0, 15)

            var htmlContant = `<div id="` + item.id + `" class="conversation-list"><a class="list-group-item list-group-item-action border-0 contact-item-list">`

            if (item.undisplayedMessages > 0) {
                if (item.undisplayedMessages < 10)
                    htmlContant += `<div class="badge bg-success float-end contanct-badge">` + item.undisplayedMessages + `</div>`
                else
                    htmlContant += `<div class="badge bg-success float-end contanct-badge">+9</div>`
            }

            htmlContant += `<div class="d-flex align-items-start">
                <img src="https://bootdey.com/img/Content/avatar/avatar5.png" class="rounded-circle me-1"
                     width="40" height="40">
                <div class="flex-grow-1 ms-3">
                ` + item.receiverName +
                `<div class="small"><span class="fas fa-circle text-success me-1"></span>` + msg + `</div>
                </div>
            </div>
            </a></div>`

            $('#div-contacts').append(htmlContant)
        })
    }

    $(document).on('click', '.conversation-list', function (e) {
        _receiverId = e.currentTarget.id

        VisualizeConversation(e.currentTarget.id)
    })

    function GetConversation(receiverId, limit, skip) {
        return new Promise(function (resolve, reject) {
            var obj = {
                receiverId: receiverId
            };

            if (skip != null) {
                obj.skip = skip;
            }

            if (limit != null) {
                obj.limit = limit;
            }

            var json = JSON.stringify(obj);

            $.ajax({
                url: urlGetConversation,
                type: "POST",
                data: { json: json },
                success: function (result) {
                    resolve(result); 
                },
                error: function (error) {
                    console.error(error);
                }
            });
        });
    }

    function VisualizeConversation(receiverId) {
        $.ajax({
            url: urlVisualizeMessages,
            type: "POST",
            data: { receiverId: receiverId },
            success: function (result) {
                GetBadgeNewMessage(receiverId)
                GetUserConversation(receiverId)
                GetAvatar(receiverId)
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    async function GetUserConversation(receiverId) {
        var result = await GetConversation(receiverId);

        $('.chat-messages').empty();

        var htmlChat = "";
        result.forEach(function (item) {
            if (item.userId == receiverId) {
                htmlChat += `<div id="` + item.id + `" class="chat-message-left pb-1"><div class="flex-shrink-1 bg-light rounded py-2 px-3 ml-3">`;
            } else {
                htmlChat += `<div id="` + item.id + `" class="chat-message-right pb-1"><div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3">`;
            }

            htmlChat += item.content + `<div class="text-muted small text-nowrap text-end">` +
                item.timestamp.substring(11, 16);

            if (item.userId != receiverId) {
                if (item.viewed != null) {
                    htmlChat += ' ✓✓';
                } else {
                    htmlChat += ' ✓';
                }
            }

            htmlChat += `</div> </div> </div>`;
        });

        $('.chat-messages').append(htmlChat);

        var htmlInput = `<div class="down">
          <div class="flex-grow-0 py-3 px-4 border-top">
            <div class="input-group">
              <input type="text" class="form-control" id="inputMessage" placeholder="Type your message">
              <button class="btn btn-primary" id="send">Send</button>
            </div>
          </div>
        </div>`;

        $('.col-md-9').append(htmlInput);

        OverflowChat()
    }

    function GetAvatar(id) {
        $.ajax({
            url: urlGetUser,
            type: "POST",
            data: { id: id },
            success: function (result) {
                var htmlAvatar = `<div class="d-flex align-items-center py-1">
                    <div class="position-relative">
                        <img src="https://bootdey.com/img/Content/avatar/avatar3.png" class="rounded-circle mr-1"
                            width="40" height="40">
                    </div>
                    <div class="flex-grow-1 pl-3">
                        <strong>` + result + `</strong>
                    </div>
                </div>`

                $('.col-md-9').prepend(htmlAvatar)
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    function GetBadgeNewMessage(receiverId) {
        $.ajax({
            url: urlGetContact,
            type: "POST",
            data: { receiverId: receiverId },
            success: function (result) {
                $("div#" + receiverId + " .contact-item-list .contanct-badge").remove()

                if (result.undisplayedMessages > 0) {
                    var htmlContant = ''

                    if (result.undisplayedMessages < 10)
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

    $(document).on('keypress', function (e) {
        if (e.which == 13 && e.target.id == "inputMessage") {
            SendMessage()
        }
    });

    $('#send').on('click', function () {
        SendMessage()
    })

    function SendMessage() {
        var txt = $("#inputMessage").val()

        if (txt != "") {
            WriteMessage(txt)

            obj = {}
            obj.receiverId = _receiverId
            obj.content = txt
            obj.timestamp = new Date()

            var json = JSON.stringify(obj)

            $.ajax({
                url: urlInsertChat,
                type: "POST",
                data: { json: json },
                success: function (result) {
                    console.log(result)
                },
                error: function (error) {
                    console.log(error)
                }
            })

            OverflowChat()
        }
    }

    function WriteMessage(txt) {
        const date = new Date();
        const hour = date.getHours();
        const min = date.getMinutes();

        var htmlMessage = `<div class="chat-message-right pb-1">
                <div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3">
                ` + txt + `
                    <div class="text-muted small text-nowrap text-end">
                    ` + hour + `:` + min + ` ✓✓
                    </div>
                </div>
            </div>`

        $(".chat-messages").append(htmlMessage)
        $("#inputMessage").val("")
    }

    OverflowChat()

    function OverflowChat() {
        var max = GetMaxHeight()
        var div = $(".chat-messages").height()

        if (div > max * 90 / 100)
            $(".chat-messages").removeClass("no-overflow")
        else
            $(".chat-messages").addClass("no-overflow")

        $('.chat-messages').scrollTop($('.chat-messages')[0].scrollHeight);
    }

    ResizeComponents()

    $(window).on('resize', function () {
        ResizeComponents()
        OverflowChat()
    })

    function ResizeComponents() {
        var maxHeight = GetMaxHeight()

        $(".chat-messages").css({ "maxHeight": maxHeight + "px" })
    }

    function GetMaxHeight() {
        var height = $(window).height()
        var div = 990

        if (height < 500)
            div = 1300
        else if (height < 700)
            div = 1200
        else if (height < 900)
            div = 1100

        var maxHeight = 830 * height / div

        return maxHeight
    }

    $('#filter').on('input', function () {
        console.log(_contancts)
        var txt = $('#filter').val().toLowerCase()

        var contactsFiltered = {}

        if (txt != "") {
            contactsFiltered = _contancts.filter(x => x.receiverName.toLowerCase().includes(txt))
        }
        else
            contactsFiltered = _contancts

        CreateContacts(contactsFiltered)
    })

    $('#test').on('click', function () {
        console.log(_receiverId)
        GetNewMassege(_receiverId)
    })

    async function GetNewMassege(receiverId) {
        var result = await GetConversation(receiverId, 1);
        console.log(result)
        if(result[0].receiverId != receiverId){
            var htmlChat = "";
            result.forEach(function (item) {
                if (item.userId == receiverId) {
                    htmlChat += `<div id="` + item.id + `" class="chat-message-left pb-1"><div class="flex-shrink-1 bg-light rounded py-2 px-3 ml-3">`;
                } else {
                    htmlChat += `<div id="` + item.id + `" class="chat-message-right pb-1"><div class="flex-shrink-1 bg-light rounded py-2 px-3 mr-3">`;
                }
    
                htmlChat += item.content + `<div class="text-muted small text-nowrap text-end">` +
                    item.timestamp.substring(11, 16);
    
                if (item.userId != receiverId) {
                    if (item.viewed != null) {
                        htmlChat += ' ✓✓';
                    } else {
                        htmlChat += ' ✓';
                    }
                }
    
                htmlChat += `</div> </div> </div>`;
            });
    
            $('.chat-messages').append(htmlChat);

            OverflowChat()
        }
    }
})