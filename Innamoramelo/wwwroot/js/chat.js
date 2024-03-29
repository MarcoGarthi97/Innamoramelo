$(document).ready(function () {
    var _receiverId = ''
    var _userId = ''

    var _contancts = []
    var _newMatches = []

    LoadComponents()

    function LoadComponents() {
        GetUserId()
        GetMatches()
        LoadCarousel()
    }

    function GetUserId() {
        $.ajax({
            url: urlGetUserId,
            type: "POST",
            success: function (result) {
                _userId = result
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    function GetMatches() {
        $.ajax({
            url: urlGetMatches,
            type: "POST",
            success: function (result) {
                $('#div-contacts').empty()
                $('#list-carousel').empty()

                CreateCarousel(result)

                result.forEach(function (item) {
                    GetContact(item)
                })
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
                _contancts = result

                CreateContacts(_contancts)
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    function GetContact(receiverId) {
        $.ajax({
            url: urlGetContact,
            type: "POST",
            data: { receiverId: receiverId },
            success: function (result) {
                console.log(result)
                if (result.content != null) {
                    _contancts.push(result)

                    CreateContact(result)
                }
                else {
                    _newMatches.push(result)

                    CreateCarouselItem(result)
                }
            },
            error: function (error) {
                console.log(error)
            }
        })
    }

    function CreateCarousel(result) {
        $('#list-carousel').empty()

        result.forEach(function (item) {
            if(item.content == null)
                CreateCarouselItem(item)
        })
    }

    function CreateCarouselItem(result) {
        var isActive = ''
        if (_newMatches.length == 1) {
            isActive = 'active'

            $("#recipeCarousel").removeAttr("data-bs-ride", "carousel");
            $("#recipeCarousel").attr("data-interval", "false");
            $("#recipeCarousel").attr("data-ride", "carousel");
            $("#recipeCarousel").attr("data-pause", "hover");
        }

        if (_newMatches.length == 11) {
            var arrowsHTML = `<a class="carousel-control-prev bg-transparent w-aut" href="#recipeCarousel"
            role="button" data-bs-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
        </a>
        <a class="carousel-control-next bg-transparent w-aut" href="#recipeCarousel"
            role="button" data-bs-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
        </a>`

            $('#recipeCarousel').append(arrowsHTML)

            $("#recipeCarousel").attr("data-bs-ride", "carousel");
            $("#recipeCarousel").removeAttr("data-interval", "false");
            $("#recipeCarousel").removeAttr("data-ride", "carousel");
            $("#recipeCarousel").removeAttr("data-pause", "hover");
        }

        var itemHTML = `<div id="carousel-item-` + result.id + `" class="carousel-item ` + isActive + `">
        <div class="col-md-3">
            <div class="card rounded-circle">
                <div class="card-img">
                    <img src="https://mdbcdn.b-cdn.net/img/new/avatars/9.webp"
                        class="img-fluid rounded-circle" alt="` + result.receiverName + `">
                </div>
            </div>
        </div>
    </div>`

        $('#list-carousel').append(itemHTML)

        console.log($('#list-carousel'))
    }

    function LoadCarousel() {
        let items = document.querySelectorAll('.carousel .carousel-item')
        console.log(items)
        items.forEach((el) => {
            const minPerSlide = 10
            let next = el.nextElementSibling
            for (var i = 1; i < minPerSlide; i++) {
                if (!next) {
                    // wrap carousel by using first child
                    next = items[0]
                }
                let cloneChild = next.cloneNode(true)
                el.appendChild(cloneChild.children[0])
                next = next.nextElementSibling
            }
        })
    }

    function CreateContacts(result) {
        $('#div-contacts').empty()

        result.forEach(function (item) {
            BuildContactToHTML(item)
        })
    }

    function CreateContact(result) {
        BuildContactToHTML(result)
    }

    function BuildContactToHTML(item) {
        var msg = ""
        if (item.isReceiverMessage)
            msg += item.receiverName + ": "
        else
            msg += "You: "

        msg += item.content.substring(0, 15)
        if (item.content.length > 14)
            msg += "..."

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
                <div class="name-contact flex-grow-1 ms-3">
                ` + item.receiverName +
            `<div class="small">` + msg + `</div>
                </div>
            </div>
            </a></div>`

        $('#div-contacts').append(htmlContant)
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

    async function GetLastMessage(senderId, isYou = false) {
        var chat = await GetConversation(senderId, 1)

        var msg = chat[0].content.substring(0, 15)
        if (chat[0].content.substring(0, 15) > 14)
            msg += '...'

        var fatherElement = $("#" + senderId);

        var childrenElement = fatherElement.find(".name-contact");
        var contactName = childrenElement.html()

        if (isYou)
            msg = "You: " + msg
        else
            msg = contactName.substring(contactName.indexOf(":")) + ": " + msg

        childrenElement = fatherElement.find(".small");
        childrenElement.html('')
        childrenElement.html(msg)

        if (_receiverId != senderId)
            CreateToast(chat[0], contactName)
    }

    function CreateToast(chat, contactName) {
        console.log(chat)

        var htmlToast = `<div class="toast" role="alert" aria-live="assertive" aria-atomic="true">
        <div class="toast-header">
          <img src="" class="rounded mr-2" alt="">
          <strong class="mr-auto">` + contactName + `</strong>
          <button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <div class="toast-body">
        ` + chat.content + `
        </div>
      </div>`

        $('#div-toast').append(htmlToast)

        $('.toast').toast('show')
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
                    connection.invoke("SendMessage", _receiverId, _userId).catch(function (err) {
                        return console.error(err.toString());
                    });
                    event.preventDefault();

                    GetLastMessage(_receiverId, true)
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
        var txt = $('#filter').val().toLowerCase()

        var contactsFiltered = {}
        var newMatchesFiltered = {}

        if (txt != "") {
            contactsFiltered = _contancts.filter(x => x.receiverName.toLowerCase().includes(txt))
            newMatchesFiltered = _newMatches.filter(x => x.receiverName.toLowerCase().includes(txt))
        }
        else {
            contactsFiltered = _contancts
            newMatchesFiltered = _newMatches
        }

        CreateContacts(contactsFiltered)
        CreateCarousel(newMatchesFiltered)
    })

    $('#test').on('click', function () {
        $('.toast').toast('show')
    })

    async function GetNewMassege(receiverId = _receiverId) {
        var result = await GetConversation(receiverId, 1);
        console.log(result)
        if (result[0].receiverId != receiverId) {
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

    "use strict";

    //DA CAMBIARE QUANDO VA SUL SERVER
    //var connection = new signalR.HubConnectionBuilder().withUrl("https://marcogarthi.com/Innamoramelo/chatHub").build();
    var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

    connection.on("GetNewMassege", function (senderId) {
        if (_receiverId == senderId)
            GetNewMassege()
        else {
            GetBadgeNewMessage(senderId)
        }

        GetLastMessage(senderId)
    });

    connection.start().then(function () {
        console.log('Ok!')
    }).catch(function (err) {
        return console.error(err.toString());
    });

    /*$(document).on('click', '#send', function(){        
        console.log('Send!')
        var user = 'pippo'
        var message = 'pippolini'    
    
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    })  */
})