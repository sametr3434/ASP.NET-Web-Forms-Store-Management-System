/* ------------------------------------------------------------------------------
 *
 *  # Custom JS code
 *
 *  Place here all your custom js. Make sure it's loaded after app.js
 *
 * ---------------------------------------------------------------------------- */


/* ------------------------------------------------------------------------------
 *
 *  # Select2 selects
 *
 *  Specific JS code additions for form_select2.html page
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var Select2Selects = function () {


    //
    // Setup module components
    //

    // Select2 examples
    var _componentSelect2 = function () {
        if (!$().select2) {
            console.warn('Warning - select2.min.js is not loaded.');
            return;
        }


        //
        // Basic examples
        //

        // Default initialization
        $('.select').select2({
            minimumResultsForSearch: Infinity
        });

        // Select with search
        $('.select-search').select2();

        // Fixed width. Single select
        $('.select-fixed-single').select2({
            minimumResultsForSearch: Infinity,
            width: 250
        });

        // Fixed width. Multiple selects
        $('.select-fixed-multiple').select2({
            minimumResultsForSearch: Infinity,
            width: 400
        });


        //
        // Advanced examples
        //

        // Minimum input length
        $('.select-minimum').select2({
            minimumInputLength: 2,
            minimumResultsForSearch: Infinity
        });

        // Allow clear selection
        $('.select-clear').select2({
            placeholder: 'Select a State',
            allowClear: true
        });

        // Tagging support
        $('.select-multiple-tags').select2({
            tags: true
        });

        // Maximum input length
        $('.select-multiple-maximum-length').select2({
            tags: true,
            maximumInputLength: 5
        });

        // Tokenization
        $('.select-multiple-tokenization').select2({
            tags: true,
            tokenSeparators: [',', ' ']
        });

        // Maximum selection
        $('.select-multiple-limited').select2({
            maximumSelectionLength: 3
        });

        // Maximum selections allowed
        $('.select-multiple-maximum').select2({
            maximumSelectionSize: 3
        });


        //
        // Drag and drop selected items
        //

        // Initialize with tags
        $('.select-multiple-drag').select2({
            containerCssClass: 'sortable-target'
        });

        //// Add jQuery UI Sortable support
        //$('.sortable-target .select2-selection__rendered').sortable({
        //    containment: '.sortable-target',
        //    items: '.select2-selection__choice:not(.select2-search--inline)'
        //});


        //
        // Single select with icons
        //

        // Format icon
        function iconFormat(icon) {
            var originalOption = icon.element;
            if (!icon.id) { return icon.text; }
            var $icon = '<i class="icon-' + $(icon.element).data('icon') + '"></i>' + icon.text;

            return $icon;
        }

        // Initialize with options
        $('.select-icons').select2({
            templateResult: iconFormat,
            minimumResultsForSearch: Infinity,
            templateSelection: iconFormat,
            escapeMarkup: function (m) { return m; }
        });


        //
        // Customize matched results
        //

        // Setup matcher
        function matchStart(term, text) {
            if (text.toUpperCase().indexOf(term.toUpperCase()) == 0) {
                return true;
            }

            return false;
        }

        // Initialize
        $.fn.select2.amd.require(['select2/compat/matcher'], function (oldMatcher) {
            $('.select-matched-customize').select2({
                minimumResultsForSearch: Infinity,
                placeholder: 'Select a State',
                matcher: oldMatcher(matchStart)
            });
        });


        //
        // Loading arrays of data
        //

        // Data
        var array_data = [
            { id: 0, text: 'enhancement' },
            { id: 1, text: 'bug' },
            { id: 2, text: 'duplicate' },
            { id: 3, text: 'invalid' },
            { id: 4, text: 'wontfix' }
        ];

        // Loading array data
        $('.select-data-array').select2({
            placeholder: 'Click to load data',
            minimumResultsForSearch: Infinity,
            data: array_data
        });


        //
        // Loading remote data
        //

        // Format displayed data
        function formatRepo(repo) {
            if (repo.loading) return repo.text;

            var markup = '<div class="select2-result-repository clearfix">' +
                '<div class="select2-result-repository__avatar"><img src="' + repo.owner.avatar_url + '" /></div>' +
                '<div class="select2-result-repository__meta">' +
                '<div class="select2-result-repository__title">' + repo.full_name + '</div>';

            if (repo.description) {
                markup += '<div class="select2-result-repository__description">' + repo.description + '</div>';
            }

            markup += '<div class="select2-result-repository__statistics">' +
                '<div class="select2-result-repository__forks">' + repo.forks_count + ' Forks</div>' +
                '<div class="select2-result-repository__stargazers">' + repo.stargazers_count + ' Stars</div>' +
                '<div class="select2-result-repository__watchers">' + repo.watchers_count + ' Watchers</div>' +
                '</div>' +
                '</div></div>';

            return markup;
        }

        // Format selection
        function formatRepoSelection(repo) {
            return repo.full_name || repo.text;
        }

        // Initialize
        $('.select-remote-data').select2({
            ajax: {
                url: 'https://api.github.com/search/repositories',
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        q: params.term, // search term
                        page: params.page
                    };
                },
                processResults: function (data, params) {

                    // parse the results into the format expected by Select2
                    // since we are using custom formatting functions we do not need to
                    // alter the remote JSON data, except to indicate that infinite
                    // scrolling can be used
                    params.page = params.page || 1;

                    return {
                        results: data.items,
                        pagination: {
                            more: (params.page * 30) < data.total_count
                        }
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            minimumInputLength: 1,
            templateResult: formatRepo, // omitted for brevity, see the source of this page
            templateSelection: formatRepoSelection // omitted for brevity, see the source of this page
        });


        //
        // Programmatic access (single)
        //

        // Set/get value
        $('.select-access-value').select2({
            minimumResultsForSearch: Infinity,
            placeholder: 'Select State...'
        });
        $('.access-get').on('click', function () { alert('Selected value is: ' + $('.select-access-value').val()); });
        $('.access-set').on('click', function () { $('.select-access-value').val('CA').trigger('change'); });


        // Open/close menu
        $('.select-access-open').select2({
            minimumResultsForSearch: Infinity,
            placeholder: 'Select State...'
        });
        $('.access-open').on('click', function () { $('.select-access-open').select2('open'); });
        $('.access-close').on('click', function () { $('.select-access-open').select2('close'); });


        // Enable/disable menu
        $('.select-access-enable').select2({
            minimumResultsForSearch: Infinity,
            placeholder: 'Select State...'
        });
        $('.access-disable').on('click', function () { $('.select-access-enable').prop('disabled', true); });
        $('.access-enable').on('click', function () { $('.select-access-enable').prop('disabled', false); });


        // Destroy/create menu
        function create_menu() {
            $('.select-access-create').select2({
                minimumResultsForSearch: Infinity,
                placeholder: 'Select State...'
            });
        }
        create_menu();
        $('.access-create').on('click', function () { return create_menu() });
        $('.access-destroy').on('click', function () { $('.select-access-create').select2('destroy'); });


        //
        // Programmatic access (multiple)
        //

        // Reacting to external value changes
        $('.select-access-multiple-value').select2();
        $('.change-to-ca').on('click', function () { $('.select-access-multiple-value').val('CA').trigger('change'); });
        $('.change-to-ak-co').on('click', function () { $('.select-access-multiple-value').val(['AK', 'CO']).trigger('change'); });


        // Open/close menu
        $('.select-access-multiple-open').select2({
            minimumResultsForSearch: Infinity
        });
        $('.access-multiple-open').on('click', function () { $('.select-access-multiple-open').select2('open'); });
        $('.access-multiple-close').on('click', function () { $('.select-access-multiple-open').select2('close'); });


        // Enable/disable menu
        $('.select-access-multiple-enable').select2({
            minimumResultsForSearch: Infinity
        });
        $('.access-multiple-disable').on('click', function () { $('.select-access-multiple-enable').prop('disabled', true); });
        $('.access-multiple-enable').on('click', function () { $('.select-access-multiple-enable').prop('disabled', false); });


        // Destroy/create menu
        function create_menu_multiple() {
            $('.select-access-multiple-create').select2({
                minimumResultsForSearch: Infinity
            });
        }
        create_menu_multiple();
        $('.access-multiple-create').on('click', function () { return create_menu_multiple() });
        $('.access-multiple-destroy').on('click', function () { $('.select-access-multiple-create').select2('destroy'); });


        // Clear selection
        $('.select-access-multiple-clear').select2({
            minimumResultsForSearch: Infinity
        });
        $('.access-multiple-clear').on('click', function () { $('.select-access-multiple-clear').val(null).trigger('change'); });
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function () {
            _componentSelect2();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    Select2Selects.init();
});


function OnayKutusuOnayAl(title, text, type, confirmButtonText, cancelButtonText, Url, target = "") {
    var sonuc = false;
    swal({
        title: title,
        text: text,
        type: type,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText
    },
        function (isConfirm) {
            if (isConfirm) {
                if (target == "") {
                    window.location.replace(Url);
                }
                else {
                    window.open(Url, target);
                }
            }
        });
    return sonuc;
}

function ButonOnayKutusuOnayAl(title, text, type, confirmButtonText, cancelButtonText, Url, target = "") {
    var sonuc = false;
    swal({
        title: title,
        text: text,
        type: type,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText
    },
        function (isConfirm) {
            if (isConfirm) {
                $(target).submit();
            }
        });
    return sonuc;
}



/* ------------------------------------------------------------------------------
 *
 *  # Summernote editor
 *
 *  Demo JS code for editor_summernote.html page
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var Summernote = function () {


    //
    // Setup module components
    //

    // Summernote
    var _componentSummernote = function () {
        if (!$().summernote) {
            console.warn('Warning - summernote.min.js is not loaded.');
            return;
        }
        $('.summernote').summernote();

    };
    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _componentSummernote();
            _componentUniform();
        }
    }
}();


// Initialize module
// ------------------------------





var ResimEkle = function (context) {
    var ui = $.summernote.ui;
    var button = ui.button({
        contents: '<i class="fa fa-picture-o"/> Resim Ekle',
        //tooltip: 'Resim Ekle',
        click: function (e) {
            EditorAcResimEkle(context);
        }
    });
    return button.render();
}
var LinkEkle = function (context) {
    var ui = $.summernote.ui;
    var button = ui.button({
        contents: '<i class="fa fa-link"/> Link Ekle',
        //tooltip: 'Link Ekle',
        click: function (e) {
            EditorAcLinkEkle(context);
        }
    });
    return button.render();
}
var VideoEkle = function (context) {
    var ui = $.summernote.ui;
    var button = ui.button({
        contents: '<i class="fa fa-youtube-play"/> Video Ekle',
        //tooltip: 'Video Ekle',
        click: function (e) {
            EditorAcVideoEkle(context);
        }
    });
    return button.render();
}
var AuidoEkle = function (context) {
    var ui = $.summernote.ui;
    var button = ui.button({
        contents: '<i class="fa fa-file-audio-o"/> Ses Ekle',
        //tooltip: 'Ses Ekle',
        click: function (e) {
            EditorAcAudioEkle(context);
        }
    });
    return button.render();
}
function EditorAcAudioEkle(editor) {
    var Folder = editor.data("userelfinderfolder");
    var subFolder = editor.data("userelfindersubfolder");
    if (Folder == null) {
        Folder = "Content";
    }
    if (subFolder == null) {
        subFolder = "Bos";
    }
    $('<div />').dialogelfinder({
        url: '/connector',
        customData: { folder: Folder, subFolder: subFolder },
        rememberLastDir: false,
        lang: 'tr',
        commandsOptions: {
            getfile: {
                oncomplete: 'destroy' // destroy elFinder after file selection
            }
        },
        getFileCallback: function (File) {
            var url = File.path.toString();
            url = url.replace('Files', File.baseUrl);
            var Html = '<p><audio controls><source src="' + url + '" type="audio/mpeg"></audio></p><p></p>'
            editor.summernote('pasteHTML', Html);
        }
    });
}
function EditorAcResimEkle(editor) {
    var Folder = editor.data("userelfinderfolder");
    var subFolder = editor.data("userelfindersubfolder");
    console.log(Folder);
    console.log(subFolder);
    if (Folder == null) {
        Folder = "Content";
    }
    if (subFolder == null) {
        subFolder = "Bos";
    }
    $('<div />').dialogelfinder({
        url: '/connector',
        customData: { folder: Folder, subFolder: subFolder },
        rememberLastDir: false,
        lang: 'tr',
        commandsOptions: {
            getfile: {
                oncomplete: 'destroy' // destroy elFinder after file selection
            }
        },
        getFileCallback: function (File) {
            var url = File.path.toString();
            url = url.replace('Files', File.baseUrl);
            console.log(url);
            editor.summernote('editor.insertImage', url, function ($image) {
                $image.css('width', $image.width());
                $image.attr('class', "img-responsive");
            });
        }
    });
}
function EditorAcVideoEkle(editor) {
    var Folder = editor.data("userelfinderfolder");
    var subFolder = editor.data("userelfindersubfolder");
    if (Folder == null) {
        Folder = "Content";
    }
    if (subFolder == null) {
        subFolder = "Bos";
    }
    $('<div />').dialogelfinder({
        url: '/connector',
        customData: { folder: Folder, subFolder: subFolder },
        rememberLastDir: false,
        lang: 'tr',
        commandsOptions: {
            getfile: {
                oncomplete: 'destroy' // destroy elFinder after file selection
            }
        },
        getFileCallback: function (File) {
            var url = File.path.toString();
            url = url.replace('Files', File.baseUrl);
            var node = document.createElement('div');
            var html = '<video width="100%" height="400" controls="controls"><source src="' + url + '" type= "video/mp4" autostart= "true"></video>';
            editor.summernote.invoke('editor.insertText', html);
        }
    });
}
function EditorAcLinkEkle(editor) {
    var Folder = editor.data("userelfinderfolder");
    var subFolder = editor.data("userelfindersubfolder");
    if (Folder == null) {
        Folder = "Content";
    }
    if (subFolder == null) {
        subFolder = "Bos";
    }
    $('<div />').dialogelfinder({
        url: '/connector',
        customData: { folder: Folder, subFolder: subFolder },
        rememberLastDir: false,
        lang: 'tr',
        commandsOptions: {
            getfile: {
                oncomplete: 'destroy' // destroy elFinder after file selection
            }
        },
        getFileCallback: function (File) {
            var url = File.path.toString();
            url = url.replace('Files', File.baseUrl);
            editor.summernote('createLink',
                {
                    text: File.name,
                    url: url,
                    newWindow: true
                }
            );
        }
    });
}
function AreaTextSummerNote(Editor) {
    var Editor = $(Editor);
    Editor.summernote({
        height: 400,
        toolbar: [
            ['style', ['style']],
            ['font', ['bold', 'underline', 'clear']],
            ['fontname', ['fontname']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['table', ['table']],
            //['insert', ['link', 'picture', 'video']],
            ['view', ['fullscreen', 'codeview', 'help']],
            ['ResimEkle', ['ResimEkle']],
            ['LinkEkle', ['LinkEkle']],
            ['VideoEkle', ['VideoEkle']],
            ['AuidoEkle', ['AuidoEkle']]
        ],
        buttons: {
            ResimEkle: ResimEkle(Editor),
            LinkEkle: LinkEkle(Editor),
            VideoEkle: VideoEkle(Editor),
            AuidoEkle: AuidoEkle(Editor)
        }
    });
}
document.addEventListener('DOMContentLoaded', function () {
    AreaTextSummerNote('.summernote');
});


(function ($) {
    var GetUrlBilgi = function (href) {
        var l = document.createElement("a");
        l.href = href;
        return l;
    };
    $(".BtnResimYukle").click(function () {
        var NesneId = $(this).attr("data-hedef");
        var Folder = $(this).attr("data-userelfinderfolder");
        var subFolder = $(this).attr("data-userelfindersubfolder");
        $('<div />').dialogelfinder({
            url: '/connector',
            customData: { folder: Folder, subFolder: subFolder },
            rememberLastDir: false,
            lang: 'tr',
            commandsOptions: { getfile: { oncomplete: 'destroy' } },
            getFileCallback: function (File) {
                console.log(File);
                var url = File.path.toString();
                url = url.replace('Files', File.baseUrl);
                var urlRsm = url;
                var Lnk = GetUrlBilgi(url);
                url = '~' + Lnk.pathname;
                var HedefNesne = $('#' + NesneId);
                console.log(HedefNesne);
                if (HedefNesne != null) {
                    url = url.replace("~C", "~/C");
                    url = url.replace("~c", "~/c");
                    HedefNesne.val(url);
                    console.log(url);
                    $('#SpnRsm' + NesneId).attr("style", 'background-image:url(' + urlRsm + '?h=70&amp;w=150&amp;mode=max&amp;scale=both);background-repeat:no-repeat;background-size:100%;width:150px;height:70px;');
                }
            }
        });
    });
    $(".BtnResimTemizle").click(function () {
        var NesneId = $(this).attr("data-hedef");
        var HedefNesne = $('#' + NesneId);
        if (HedefNesne != null) {
            HedefNesne.val('');
            $('#SpnRsm' + NesneId).attr("style", "width:150px;height:70px;");
        }
    });


            


}).apply(this, [jQuery]);

