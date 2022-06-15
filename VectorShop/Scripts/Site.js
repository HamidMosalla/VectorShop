/// <reference path="jquery-1.10.2.intellisense.js" />
/// <reference path="jquery-1.10.2.js" />


(function () {


    var successfulDeleteNotice = function () {
        new PNotify({
            title: 'حذف موفق',
            text: 'آیتم مورد نظر با موفقیت حذف شد.',
            type: 'success',
            icon: 'glyphicon glyphicon-ok',
            delay: 1000
        });
    }

    //code dealing with initializing the mega menu
    $(function() {
        $(".megamenu").megamenu();
    });

    //code dealing with highlighting the current link in navigation
    $(function () {

        if (window.location == window.location.origin + "/") { $("#homeActive").addClass("active"); }

        $(".myNavbarActive a").each(function (index) {
            if (this.href.trim() == window.location) {
                $(this).parent().addClass("active");
            }
        });

    });

    //code dealing with highlighting the currnt link admin menu navigation
    $(function () {

        //$("#menu-content li").find("[data-target='" + liId + "']").addClass("active");

        $("#menu-content a").each(function (index) {

            if (this.href.trim() == window.location) {
                $(this).parent().addClass("active");
                $(this).closest("ul").removeClass("collapse");
                $(this).closest("ul").prev("li").addClass("active");
            }

        });

    });

    //code dealing with search page and email select page and keeping the sorting select value
    $(function () {

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            var results = regex.exec(window.location.href);
            if (results == null)
                return "";
            else
                return decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        $("#proSort option[value='" + getParameterByName('proSort') + "']").prop('selected', true);
        $("#catSort option[value='" + getParameterByName('catSort') + "']").prop('selected', true);
        $("#priCat option[value='" + getParameterByName('priCat') + "']").prop('selected', true);
        $("#secCat option[value='" + getParameterByName('secCat') + "']").prop('selected', true);
    });

    //code dealing with visually deleting the parent and child category from category page.
    $(function () {

        //all anchors with class of deleteLink have their URL set to post
        //with $.post we post to this URL and accept the delete instead of
        //URL not found than we use the states code we set in controller
        //and fade and remove the link that was clicked from the DOM
        //and than we check to see if link was parent if it was we grab
        //its children by the class that we set for them and fade and remove
        //them from the DOM, beside you can also use //$.post(item.attr("href"), function (data) {}, timmy!

        $("a.deleteLink").click(function (e) {

            e.preventDefault();

            var item = $(this);
            var link = item.attr("href");
            var token = $('input[name="__RequestVerificationToken"]').val();

            (new PNotify({
                title: 'تایید حذف',
                text: 'آیا از حذف دسته بندی مورد نظر اطمینان دارید؟',
                icon: 'glyphicon glyphicon-question-sign',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            })).get().on('pnotify.confirm', function () {

                $.ajax({
                    url: link,
                    type: 'POST',
                    data: { __RequestVerificationToken: token },
                    dataType: 'json',
                    success: function (data) {

                        if (data.Status === "Deleted") {
                            successfulDeleteNotice();
                            item.closest("tr").fadeOut("slow").remove();

                            if (data.CatType === "PriCat") {
                                $("a." + data.CatId).each(function () {

                                    $(this).closest("tr").fadeOut("slow").remove();

                                });

                            }
                        }

                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.responseText);
                        alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                    }
                });

            });

        });

    });

    //code dealing with fileupload on Create product page responsible for its shape
    $(function () {
        !function (e) {
            var t = function (t, n) {
                this.$element = e(t), this.type = this.$element.data("uploadtype") || (this.$element.find(".thumbnail").length > 0 ? "image" : "file"), this.$input = this.$element.find(":file");
                if (this.$input.length === 0) return;
                this.name = this.$input.attr("name") || n.name, this.$hidden = this.$element.find('input[type=hidden][name="' + this.name + '"]'), this.$hidden.length === 0 && (this.$hidden = e('<input type="hidden" />'), this.$element.prepend(this.$hidden)), this.$preview = this.$element.find(".fileupload-preview");
                var r = this.$preview.css("height");
                this.$preview.css("display") != "inline" && r != "0px" && r != "none" && this.$preview.css("line-height", r), this.original = { exists: this.$element.hasClass("fileupload-exists"), preview: this.$preview.html(), hiddenVal: this.$hidden.val() }, this.$remove = this.$element.find('[data-dismiss="fileupload"]'), this.$element.find('[data-trigger="fileupload"]').on("click.fileupload", e.proxy(this.trigger, this)), this.listen()
            };
            t.prototype = {
                listen: function () { this.$input.on("change.fileupload", e.proxy(this.change, this)), e(this.$input[0].form).on("reset.fileupload", e.proxy(this.reset, this)), this.$remove && this.$remove.on("click.fileupload", e.proxy(this.clear, this)) },
                change: function (e, t) {
                    if (t === "clear") return;
                    var n = e.target.files !== undefined ? e.target.files[0] : e.target.value ? { name: e.target.value.replace(/^.+\\/, "") } : null;
                    if (!n) {
                        this.clear();
                        return
                    }
                    this.$hidden.val(""), this.$hidden.attr("name", ""), this.$input.attr("name", this.name);
                    if (this.type === "image" && this.$preview.length > 0 && (typeof n.type != "undefined" ? n.type.match("image.*") : n.name.match(/\.(gif|png|jpe?g)$/i)) && typeof FileReader != "undefined") {
                        var r = new FileReader, i = this.$preview, s = this.$element;
                        r.onload = function (e) { i.html('<img src="' + e.target.result + '" ' + (i.css("max-height") != "none" ? 'style="max-height: ' + i.css("max-height") + ';"' : "") + " />"), s.addClass("fileupload-exists").removeClass("fileupload-new") }, r.readAsDataURL(n)
                    } else this.$preview.text(n.name), this.$element.addClass("fileupload-exists").removeClass("fileupload-new")
                },
                clear: function (e) {
                    this.$hidden.val(""), this.$hidden.attr("name", this.name), this.$input.attr("name", "");
                    if (navigator.userAgent.match(/msie/i)) {
                        var t = this.$input.clone(!0);
                        this.$input.after(t), this.$input.remove(), this.$input = t
                    } else this.$input.val("");
                    this.$preview.html(""), this.$element.addClass("fileupload-new").removeClass("fileupload-exists"), e && (this.$input.trigger("change", ["clear"]), e.preventDefault())
                },
                reset: function (e) { this.clear(), this.$hidden.val(this.original.hiddenVal), this.$preview.html(this.original.preview), this.original.exists ? this.$element.addClass("fileupload-exists").removeClass("fileupload-new") : this.$element.addClass("fileupload-new").removeClass("fileupload-exists") },
                trigger: function (e) { this.$input.trigger("click"), e.preventDefault() }
            }, e.fn.fileupload = function (n) {
                return this.each(function () {
                    var r = e(this), i = r.data("fileupload");
                    i || r.data("fileupload", i = new t(this, n)), typeof n == "string" && i[n]()
                })
            }, e.fn.fileupload.Constructor = t, e(document).on("click.fileupload.data-api", '[data-provides="fileupload"]', function (t) {
                var n = e(this);
                if (n.data("fileupload")) return;
                n.fileupload(n.data());
                var r = e(t.target).closest('[data-dismiss="fileupload"],[data-trigger="fileupload"]');
                r.length > 0 && (r.trigger("click.fileupload"), t.preventDefault())
            })
        }(window.jQuery);
    });

    //code dealing with pricat dropdown and seccat dropdown on Create product page
    $(function () {

        $("#PriCatIDfk").on("change", function () {

            if (this.value !== "") {

                var listValue = this.value;
                var processMessage = "<option value='0'> لطفا کمی صبر کنید...</option>";
                $("#SecCatIDfk").html(processMessage).show();
                //make a call to our controller function and retrieve pricat's child (secCats) based on
                //value selected in pricat dropdownlist
                var url = "/Product/GetSecCatByPriCatId/";
                //we send this value through query string with jquery ajax data att and
                //url come someting like " Product/GetSecCatByPriCatId ? priCatId = listValue "
                $.ajax({
                    url: url,
                    data: { priCatId: listValue },
                    cache: false,
                    type: "POST",
                    success: function (data) {
                        var markup = "<option value='0'>لطفا یک دسته بندی فرعی انتخاب کنید</option>";
                        if (data.length === 0) {

                            markup = "<option value='0'>فاقد دسته بندی فرعی</option>";

                        }


                        for (var i = 0; i < data.length; i++) {

                            markup += "<option value=" + data[i].Value + ">" + data[i].Text + "</option>";
                        }

                        $("#SecCatIDfk").html(markup).show();

                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.responseText);
                        alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                    }
                });


            }
        });

    });

    //code dealing with product index page loadMore ajaxCall and smooth scroll down
    $(function () {

        var loadCount = 1;
        var ajaxModelHitCount = 1;
        var loading = $("#loading");
        $("#loadMore").on("click", function (e) {

            e.preventDefault();

            $(document).on({

                ajaxStart: function () {
                    loading.show();
                },
                ajaxStop: function () {
                    loading.hide();
                }
            });




            var url = "/Product/LoadMoreProduct/";
            $.ajax({
                url: url,
                data: { size: loadCount * 4 },
                cache: false,
                type: "POST",
                success: function (data) {

                    //$("#vectorShopBody").append(data.ModelString).hide().show("slow"); //Doesn't work //
                    if (data.length !== 0) {
                        $(data.ModelString).insertBefore("#loadMore").hide().fadeIn(2000);
                    }

                    var ajaxModelCount = data.ModelCount - (ajaxModelHitCount * 4);
                    if (ajaxModelCount <= 0) {
                        $("#loadMore").hide().fadeOut(2000);
                    }

                },
                error: function (xhr, status, error) {
                    console.log(xhr.responseText);
                    alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                }
            });/*.done(function () {
                var n = $(document).height() - 1370;
                $('html, body').animate({ scrollTop: n }, 2000);
            })*/


            ajaxModelHitCount = ajaxModelHitCount + 1;
            loadCount = loadCount + 1;

        });

    });

    //code dealing with visually active the selected pagination button on product management page
    $(function () {

        var selectedPage = $("#PaginationIndex").data("pageindex");

        $(".PaginationAnchor a").each(function () {

            if ($(this).text() == selectedPage) {
                $(this).parent().addClass("active");
            }

        });

    });

    //code dealing with visually deleting the slideshow
    $(function () {

        $(".SlideShowDeleteLink").on("click", function (e) {

            e.preventDefault();

            var token = $('input[name="__RequestVerificationToken"]').val();
            var item = $(this);
            var link = item.attr("href");

            (new PNotify({
                title: 'تایید حذف',
                text: 'آیا از حذف اسلاید مورد نظر اطمینان دارید؟',
                icon: 'glyphicon glyphicon-question-sign',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            })).get().on('pnotify.confirm', function () {

                $.ajax({
                    url: link,
                    data: { __RequestVerificationToken: token },
                    type: 'POST',
                    dataType: 'json',
                    success: function (data) {

                        if (data.Status === "Deleted") {
                            successfulDeleteNotice();
                            item.closest("tr").fadeOut(2000);
                        }

                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.responseText);
                        alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                    }
                });

            });



        });

    });

    //code dealing with posting contact form data to create action of contact controller
    $(function () {
        //for multiple form $(document).on("submit",'form.remember' , function (e)
        //$(document).on("submit", function (e)

        $("#ContactForm").on("submit", function (e) {
            e.preventDefault();
            var token = $('input[name="__RequestVerificationToken"]').val();
            var form = $("form");

            var loading = $("#loading");
            $(document).on({

                ajaxStart: function () {
                    loading.show();
                },
                ajaxStop: function () {
                    loading.hide();
                }
            });

            if (form.valid()) {
                $.ajax({
                    url: "/Contact/CreateAjax",
                    data: form.serialize() + '&' + { __RequestVerificationToken: token },
                    type: "POST",
                    dataType: "json",
                    success: function (data) {

                        if (data.Status == "Success") {
                            new PNotify({
                                title: 'ثبت موفق',
                                text: 'پیام شما با موفقیت ثبت شد. هم اکنون به صفحه ی اصلی هدایت می شوید.',
                                type: 'success',
                                icon: 'glyphicon glyphicon-ok',
                                delay: 3000
                            });
                            setTimeout(function () { window.location.replace("/Home/Index"); }, 3000);

                        }
                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.responseText);
                        alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                    }
                });
            }
        });


    });

    //code dealing with posting NewsLetter form data to SubscribeAjax action of NewsLetter controller
    $(function () {
        //for multiple form $(document).on("submit",'form.remember' , function (e)
        //$(document).on("submit", function (e)

        $("#SubscribeForm").on("submit", function (e) {
            e.preventDefault();
            var token = $('input[name="__RequestVerificationToken"]').val();
            var form = $("form");

            var loading = $("#loading");
            $(document).on({

                ajaxStart: function () {
                    loading.show();
                },
                ajaxStop: function () {
                    loading.hide();
                }
            });

            if (form.valid()) {
                $.ajax({
                    url: "/NewsLetter/SubscribeAjax",
                    data: form.serialize() + '&' + { __RequestVerificationToken: token },
                    type: "POST",
                    dataType: "json",
                    success: function (data) {

                        if (data.Status == "Success") {
                            new PNotify({
                                title: 'ثبت موفق',
                                text: 'شما با موفقیت به عضویت خبرنامه در آمدید. هم اکنون به صفحه ی اصلی هدایت می شوید.',
                                type: 'success',
                                icon: 'glyphicon glyphicon-ok',
                                delay: 3000
                            });
                            setTimeout(function () { window.location.replace("/Home/Index"); }, 3000);

                        }
                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.responseText);
                        alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                    }
                });
            }
        });


    });

    //code dealing with visually deleting the contact
    $(function () {

        $(".ContactDeleteLink").on("click", function (e) {

            e.preventDefault();

            var token = $('input[name="__RequestVerificationToken"]').val();
            var item = $(this);
            var link = item.attr("href");

            (new PNotify({
                title: 'تایید حذف',
                text: 'آیا از حذف تماس مورد نظر اطمینان دارید؟',
                icon: 'glyphicon glyphicon-question-sign',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            })).get().on('pnotify.confirm', function () {

                $.ajax({
                    url: link,
                    data: { __RequestVerificationToken: token },
                    type: 'POST',
                    dataType: 'json',
                    success: function (data) {

                        if (data.Status === "Deleted") {
                            successfulDeleteNotice();
                            item.closest("tr").fadeOut(2000);
                        }

                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.responseText);
                        alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                    }
                });

            });



        });

    });

    //code dealing with visually deleting the link
    $(function () {

        $(".LinkDeleteLink").on("click", function (e) {

            e.preventDefault();

            var token = $('input[name="__RequestVerificationToken"]').val();
            var item = $(this);
            var link = item.attr("href");

            (new PNotify({
                title: 'تایید حذف',
                text: 'آیا از حذف تماس مورد نظر اطمینان دارید؟',
                icon: 'glyphicon glyphicon-question-sign',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            })).get().on('pnotify.confirm', function () {

                $.ajax({
                    url: link,
                    data: { __RequestVerificationToken: token },
                    type: 'POST',
                    dataType: 'json',
                    success: function (data) {

                        if (data.Status === "Deleted") {
                            successfulDeleteNotice();
                            item.closest("tr").fadeOut(2000);
                        }

                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.responseText);
                        alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                    }
                });

            });



        });

    });

    //code dealing with visually deleting the Advert
    $(function () {

        $(".AdvertDeleteLink").on("click", function (e) {

            e.preventDefault();

            var token = $('input[name="__RequestVerificationToken"]').val();
            var item = $(this);
            var link = item.attr("href");

            (new PNotify({
                title: 'تایید حذف',
                text: 'آیا از حذف تبلیغ مورد نظر اطمینان دارید؟',
                icon: 'glyphicon glyphicon-question-sign',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            })).get().on('pnotify.confirm', function () {

                $.ajax({
                    url: link,
                    data: { __RequestVerificationToken: token },
                    type: 'POST',
                    dataType: 'json',
                    success: function (data) {

                        if (data.Status === "Deleted") {
                            successfulDeleteNotice();
                            item.closest("tr").fadeOut(2000);
                        }

                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.responseText);
                        alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                    }
                });

            });



        });

    });

    //code dealing with visually deleting the NewDesignOrder
    $(function () {

        $(".NewDesignOrderDeleteLink").on("click", function (e) {

            e.preventDefault();

            var token = $('input[name="__RequestVerificationToken"]').val();
            var item = $(this);
            var link = item.attr("href");

            (new PNotify({
                title: 'تایید حذف',
                text: 'آیا از حذف تبلیغ مورد نظر اطمینان دارید؟',
                icon: 'glyphicon glyphicon-question-sign',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            })).get().on('pnotify.confirm', function () {

                $.ajax({
                    url: link,
                    data: { __RequestVerificationToken: token },
                    type: 'POST',
                    dataType: 'json',
                    success: function (data) {

                        if (data.Status === "Deleted") {
                            successfulDeleteNotice();
                            item.closest("tr").fadeOut(2000);
                        }

                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.responseText);
                        alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                    }
                });

            });



        });

    });

    //code dealing with visually deleting the Article
    $(function () {

        $(".ArticleDeleteLink").on("click", function (e) {

            e.preventDefault();

            var token = $('input[name="__RequestVerificationToken"]').val();
            var item = $(this);
            var link = item.attr("href");

            (new PNotify({
                title: 'تایید حذف',
                text: 'آیا از حذف تبلیغ مورد نظر اطمینان دارید؟',
                icon: 'glyphicon glyphicon-question-sign',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            })).get().on('pnotify.confirm', function () {

                $.ajax({
                    url: link,
                    data: { __RequestVerificationToken: token },
                    type: 'POST',
                    dataType: 'json',
                    success: function (data) {

                        if (data.Status === "Deleted") {
                            successfulDeleteNotice();
                            item.closest("tr").fadeOut(2000);
                        }

                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.responseText);
                        alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                    }
                });

            });



        });

    });

    //code dealing with visually deleting the NewsLetter
    $(function () {

        $(".NewsLetterDeleteLink").on("click", function (e) {

            e.preventDefault();

            var token = $('input[name="__RequestVerificationToken"]').val();
            var item = $(this);
            var link = item.attr("href");

            (new PNotify({
                title: 'تایید حذف',
                text: 'آیا از حذف عضو مورد نظر از خبرنامه اطمینان دارید؟',
                icon: 'glyphicon glyphicon-question-sign',
                hide: false,
                confirm: {
                    confirm: true
                },
                buttons: {
                    closer: false,
                    sticker: false
                },
                history: {
                    history: false
                }
            })).get().on('pnotify.confirm', function () {

                $.ajax({
                    url: link,
                    data: { __RequestVerificationToken: token },
                    type: 'POST',
                    dataType: 'json',
                    success: function (data) {

                        if (data.Status === "Deleted") {
                            successfulDeleteNotice();
                            item.closest("tr").fadeOut(2000);
                        }

                    },
                    error: function (xhr, status, error) {
                        console.log(xhr.responseText);
                        alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                    }
                });

            });



        });

    });

    //code dealing with selecting the Emails
    $(function () {

        // start code for check and unchecking the email when all of them are selected
        $("#selectAllEmail").prop("checked", $('.SelectEmailPage:checked').length == $('.SelectEmailPage').length);
        $(".SelectEmailPage").change(function () {
            if ($('.SelectEmailPage:checked').length == $('.SelectEmailPage').length) {
                $("#selectAllEmail").prop("checked", true);
            } else {
                $("#selectAllEmail").prop("checked", false);
            }
        });
        //end of for check and unchecking the email when all of them are selected


        $(".SelectEmailPage").on("change", function () {

            var token = $('input[name="__RequestVerificationToken"]').val();
            var item = $(this);
            var emailId = $(this).data("emailid");

            $.ajax({
                url: "/NewsLetter/SelectEmail/" + emailId,
                data: { __RequestVerificationToken: token },
                type: 'POST',
                dataType: 'json',
                success: function (data) {

                    if (data.Status === "Success") {
                        new PNotify({
                            title: 'تغییر موفقیت آمیز',
                            text: 'ایمیل مورد نظر با موفقیت به لیست حذف یا اضافه شد.',
                            type: 'success',
                            icon: 'glyphicon glyphicon-ok',
                            delay: 500
                        });
                    }

                },
                error: function (xhr, status, error) {
                    console.log(xhr.responseText);
                    alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                }
            });

        });

        $("#selectAllEmail").on("change", function () {

            var token = $('input[name="__RequestVerificationToken"]').val();
            var item = this;

            var emailIdList = new Array();
            $(".SelectEmailPage").each(function () {
                emailIdList.push($(this).data("emailid"));
            });

            if (this.checked) {
                $(".SelectEmailPage").each(function () {
                    this.checked = true;
                });
            } else {
                $(".SelectEmailPage").each(function () {
                    this.checked = false;
                });
            }

            $.ajax({
                url: "/NewsLetter/SelectAllEmail/",
                data: { __RequestVerificationToken: token, Id: emailIdList, isChecked: item.checked },
                type: 'POST',
                dataType: 'json',
                success: function (data) {

                    if (data.Status === "Success") {
                        new PNotify({
                            title: 'تغییر موفقیت آمیز',
                            text: 'ایمیل های مورد نظر با موفقیت به لیست حذف یا اضافه شدند.',
                            type: 'success',
                            icon: 'glyphicon glyphicon-ok',
                            delay: 500
                        });
                    }

                },
                error: function (xhr, status, error) {
                    console.log(xhr.responseText);
                    alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                }
            });

        });

    });


})();