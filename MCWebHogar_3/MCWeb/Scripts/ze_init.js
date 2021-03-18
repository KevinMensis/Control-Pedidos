/* $Id$ */
function ZE_Init() {
}

ZE_Init.init = function () {
    /*static variable initilisation*/
    ZE_Init.staticVersion = "v57";      //No I18N

    /*supported themes
   *
   *        blue,green,gray,brown,lavender,pink,stars,heart,wood,leaf
   *
  **/
    ZE_Init.theme = "gray";//specify your theme here //No I18N

    /* language for i18ned*/
    /* possible values
       *
       * 	english   --> en,
       * 	chinese   --> zh,
       * 	Danish    --> da,
       * 	Dutch     --> nl,
       * 	French    --> fr,
       * 	German    --> de,
       * 	hungarian --> hu,
       * 	italian   --> it,
       * 	japanese  --> ja,
       * 	polish    --> pl,
       *  portugese --> pt,
       * 	russian   --> ru,
       * 	spanish   --> es,
       *  swedish   --> sv,
       *  turkish   --> tr,
       *  ukranian  --> uk
     *
       **/

    ZE_Init.language = currLocal;//Need to be told

    /*Configuration for whether plaintext needed or not is BOOLEAN*/
    ZE_Init.needplaintext = true;

    /*Zeditor.css should be loaded inside the iframe or not*/
    ZE_Init.editorCSS = true;

    /* This parameter is to perform the RTL changes for the appearance of the editor where the orientation of the editor will appear from Right to Left.*/
    ZE_Init.isRTL = false;

    /*It specific for ManageEngine products where spellcheck is available locally. If used in ME set the value as true. */
    ZE_Init.needLocalSpellCheck = false;

    /*Need Inline quotes*/
    ZE_Init.inlineQuotes = false;

    /*Notifying mode change for mail team which argument will give the mode*/
    ZE_Init.modeChange = undefined;

    /*spellcheck url by default it will be lt.zoho.com*/
    ZE_Init.spellcheckURL = "lt.zoho.com";//No I18N
    //ZE_Init.spellcheckURL = "antispam-centos-3.csez.zohocorpin.com:7777";

    /*setting domain name by default it will be zoho.com*/
    ZE_Init.domain = document.domain;//No I18N
    //ZE_Init.domain = "zohocorpin.com";

    /* tab key handling needed or not */
    ZE_Init.tabKeyHandling = true;

    /* focus needed or not for editor */
    ZE_Init.needEditorFocus = true;

    /* ze border needed or not start*/
    ZE_Init.needEditorBorder = true;

    /* Image resizing needed or not start*/
    ZE_Init.needResizeImage = false;

    /* removing the insert options on close*/
    ZE_Init.removeInsertOptions = false;

    /* removing the fontfamily on close*/
    ZE_Init.removeFontFamily = false;

    /* removing the fontsize on close*/
    ZE_Init.removeFontSize = false;

    /* Default font-family to be applied on the document inside the iframe */
    ZE_Init.defaultFontSize = "10pt";//No I18N

    /* Default font-size to be applied on the document inside the iframe */
    ZE_Init.defaultFontFamily = "arial,Verdana,Helvetica,sans-serif";//No I18N

    /* Default font-color to be applied on the document inside the iframe */
    ZE_Init.defaultFontColor = "black";//No I18N

    /*For mail team, sets the class on editor depending upon the the option user has selected from mail settings*/
    ZE_Init.outGoingFontFamily = "";

    ZE_Init.outGoingFontSize = "";   //Enter value here in pt not px .. 

    ZE_Init.outGoingColor = "";
    ZE_Init.contextVal = "";
    ZE_Init.csrfParamVal = csrfParamName;
    ZE_Init.csrfCookieVal = csrfToken;

    /*******************************************************
  
    /* static path configuration start*/
    var loc_Protocol = window.location.protocol;
    ZE_Init.cssPath = ZE_Init.imgPath = loc_Protocol + "//" + cssStaticDomain + "/ze/" + ZE_Init.staticVersion;
    ZE_Init.jsPath = loc_Protocol + "//" + jsStaticDomain + "/ze/" + ZE_Init.staticVersion;

    /*for disabling the static start*/
    //ZE_Init.cssPath = ZE_Init.jsPath = ZE_Init.imgPath = "/ze";
    //ZE_Init.cssPath = ZE_Init.jsPath = ZE_Init.imgPath = loc_Protocol+"//localzohostatic.com/ze/v7";
    /*for disabling the static end*/

    /* very important depending on which toolbar will be generated*/
    ZE_Init.toolbarOrder = [
                              [
                                ["bold", "Bold (Ctrl+B)", "ze_tb"],//No I18N
                                ["italic", "Italic (Ctrl+I)", "ze_ti"],//No I18N
                                ["underline", "Underline (Ctrl+U)", "ze_tu"],//No I18N
                                /*["strikethrough","Strikethrough","ze_ts"],//No I18N
                                ["subscript","Subscript","ze_tsu"],//No I18N
                                ["superscript","Superscript","ze_sup"]//No I18N*/
                              ],
                              [
                                ["fontcolor", "Font Color", "ze_f"],//No I18N
                                ["bgcolor", "Background Color", "ze_tbg"],//No I18N
                                ["fontfamily", "Font", "ze_tf"],//No I18N
                                ["fontsize", "Font Size", "ze_tfo"],//No I18N
                                /*["heading","Heading","ze_hd"]//No I18N*/
                              ],
                              [
                                ["justifyleft", "Align Text Left (Ctrl+L)", "ze_tjl"],//No I18N
                                ["justifyright", "Align Text Right (Ctrl+R)", "ze_tjr"],//No I18N
                                ["justifyfull", "Justify (Ctrl+J)", "ze_tjf"],//No I18N
                                ["justifycenter", "Center (Ctrl+E)", "ze_tjc"]//No I18N
                              ],
                              [
                                ["insertunorderedlist", "Bullets", "ze_tul"],//No I18N
                                ["insertorderedlist", "Numbering", "ze_tol"],//No I18N
                                ["outdent", "Decrease Indent", "ze_tou"],//No I18N
                                ["indent", "Increase Indent", "ze_tin"]//No I18N
                              ],
                              [
                                ["removeformat", "Remove Formatting", "ze_trf"]//No I18N
                              ],
                              [
                                ["image", "Insert Image", "ze_tim"],//No I18N
                                ["smiley", "Insert smiley", "ze_tis"],//No I18N
                                ["link", "Insert Link", "ze_til"],//No I18N
                                ["unlink", "Remove Link", "ze_tuli"],//No I18N
                                ["insertoptions", "Insert Options", "ze_spo"]//No I18N
                              ],//No I18N
                              [
                                /*["table","Insert Table","ze_tbl"],//No I18N
                                ["inserthorizontalrule","Insert Horizontal Rule","ze_hr"],//No I18N
                                ["object","Insert HTML","ze_obj"],//No I18N*/
                                ["code", "Insert Code", "ze_icode"],//No I18N
                                ["quote", "Insert Quote", "ze_quote"]//No I18N
                              ],
                              [
                                ["edithtml", "Edit HTML", "ze_teh"]//No I18N
                              ],
                              /*[
                                ["spellcheck","Check Spelling","ze_sp"],//No I18N
                                ["spellcheckoptions","Spell Check Languages","ze_spo"]//No I18N
                              ]*/
    ];

    /*Insert options*/
    ZE_Init.insertOptions = [
                              /*["image","Insert Image","ze_tim"],//No I18N
                              ["smiley","Insert smiley","ze_tis"],//No I18N
                              ["link","Insert Link","ze_til"],//No I18N
                              ["unlink","Remove Link","ze_tuli"],//No I18N*/
                              ["table", "Insert Table", "ze_tbl"],//No I18N
                              ["inserthorizontalrule", "Insert Horizontal Rule", "ze_hr"],//No I18N
                              ["object", "Insert HTML", "ze_obj"],//No I18N
                              /*["code","Insert Code","ze_icode"],//No I18N
                              ["quote","Insert Quote","ze_quote"]//No I18N*/
    ];

    /*upload Image action*/
    ZE_Init.imgAction = "/zeUploadImage.do";//No I18N

    /* additional parameters to be passed for uploading image*/
    ZE_Init.imgParameters = {};
    ZE_Init.imgParameters.forumGroupId = forumGroupId;
    ZE_Init.imgParameters[csrfParamName] = csrfToken;

    ZE_Init.maxImageSize = maxAttachmentSize;
    ZE_Init.sizeErrorMsg = "Attachment size exceeds maximum limit";//No I18N

    /*If html tags passed in setContent method needs to be replaced*/
    ZE_Init.setContentProcessed = false;

    /* will be calculated during run time*/
    /*
  ZE_Init.imgParametersFunction = function()
  {
          ZE_Init.imgParameters = document.getElementById("hemaravind").style.height;
  };*/

    var _document = document,
      agt = navigator.userAgent.toLowerCase(),
      preload_image = _document.createElement("img");

    ZE_Init.is_ie = (agt.indexOf("ie") !== -1);

    ZE_Init.is_safari = (agt.indexOf("safari") !== -1)

    ZE_Init.is_opera = (agt.indexOf("opera") !== -1);

    ZE_Init.is_mac = (agt.indexOf("mac") !== -1);

    ZE_Init.theme = ZE_Init.ElementInArray(["blue", "green", "gray", "brown", "lavender", "pink", "stars", "heart", "wood", "leaf"], ZE_Init.theme) || "gray";//No I18N

    ZE_Init.language = ZE_Init.ElementInArray(["en", "zh", "da", "nl", "fr", "de", "hu", "it", "ja", "pl", "pt", "ru", "es", "sv", "tr", "uk"], ZE_Init.language) || "en";

    /*initial css added*/
    ZE_Init.loadURL(ZE_Init.cssPath + '/css/' + ZE_Init.theme + '/ze.min.css', "css");//No I18N

    /*Initial js added*/
    if (ZE_Init.is_ie) {
        ZE_Init.loadURL(ZE_Init.jsPath + "/js/i18n/" + ZE_Init.language + "/ze_ie.min.js", "js");//No I18N
    }
    else if (ZE_Init.is_safari) {
        ZE_Init.loadURL(ZE_Init.jsPath + "/js/i18n/" + ZE_Init.language + "/ze_sa.min.js", "js");//No I18N
    }
    else if (ZE_Init.is_opera) {
        ZE_Init.loadURL(ZE_Init.jsPath + "/js/i18n/" + ZE_Init.language + "/ze_op.min.js", "js");//No I18N
        ZE_Init.toolbarOrder.pop();//for opera no spellcheck support
    }
    else {
        ZE_Init.loadURL(ZE_Init.jsPath + "/js/i18n/" + ZE_Init.language + "/ze.min.js", "js");//No I18N
    }

    /*Preloading of toolbar image*/
    preload_image.src = ZE_Init.imgPath + "/images/ze_toolbar_" + ZE_Init.theme + ".png";

    /*variable hold to load ondemand js and css*/
    ZE_Init.loading = true;
};

ZE_Init.loadURL = function (URL, type) {
    var css,
    _script,
    _document = document;

    if (type === "css") {
        css = _document.createElement("link");
        css.type = 'text/css';
        css.rel = 'stylesheet';
        css.href = URL;
        _document.getElementsByTagName("head")[0].appendChild(css);
    }
    else if (type === "js") {
        _script = _document.createElement("script");
        _script.type = "text/javascript";
        _script.src = URL;
        _document.getElementsByTagName("head")[0].appendChild(_script);
    }
};

ZE_Init.ElementInArray = function (array, value) {
    var element;
    while (element = array.shift()) {
        if (element === value) {
            return element;
        }
    }
};

ZE_Init.applyTheme = function (theme) {
    /*current theme and new theme are same*/
    if (ZE_Init.theme === theme) {
        return;
    }

    /*check whether the theme available in the supported theme list*/
    if (!ZE_Init.ElementInArray(["blue", "green", "gray", "brown", "lavender", "pink", "stars", "heart", "wood", "leaf"], theme)) {
        return;
    }

    var _document = document,
    link = _document.getElementsByTagName("link"),
    _length = link.length,
    _individual_link,
    _individual_link_href,
    preload_image;

    while (_length--) {
        _individual_link = link[_length];
        _individual_link_href = _individual_link.href;
        if (_individual_link_href.match("ze.min.css")) {
            ZE_Init.theme = theme;
            _individual_link.href = ZE_Init.cssPath + '/css/' + theme + '/ze.min.css';
            preload_image = _document.createElement("img");
            preload_image.src = ZE_Init.imgPath + "/images/ze_toolbar_" + theme + ".png";
        }
        else if (_individual_link_href.match("ze1.min.css")) {
            ZE_Init.theme = theme;
            _individual_link.href = ZE_Init.cssPath + '/css/' + theme + '/ze1.min.css';
        }
    }
};

ZE_Init.toolbarOrderMapping = {
    "bold": ["bold", "Bold (Ctrl+B)", "ze_tb"],
    "italic": ["italic", "Italic (Ctrl+I)", "ze_ti"],
    "underline": ["underline", "Underline (Ctrl+U)", "ze_tu"],
    /*"strikethrough":["strikethrough","Strikethrough","ze_ts"],
    "subscript":["subscript","Subscript","ze_tsu"],
    "superscript":["superscript","Superscript","ze_sup"],*/
    "fontcolor": ["fontcolor", "Font Color", "ze_f"],
    "bgcolor": ["bgcolor", "Background Color", "ze_tbg"],
    "fontfamily": ["fontfamily", "Font", "ze_tf"],
    "fontsize": ["fontsize", "Font Size", "ze_tfo"],
    "justifyleft": ["justifyleft", "Align Text Left (Ctrl+L)", "ze_tjl"],
    "justifyright": ["justifyright", "Align Text Right (Ctrl+R)", "ze_tjr"],
    "justifyfull": ["justifyfull", "Justify (Ctrl+J)", "ze_tjf"],
    "justifycenter": ["justifycenter", "Center (Ctrl+E)", "ze_tjc"],
    "insertunorderedlist": ["insertunorderedlist", "Bullets", "ze_tul"],
    "insertorderedlist": ["insertorderedlist", "Numbering", "ze_tol"],
    "outdent": ["outdent", "Decrease Indent", "ze_tou"],
    "indent": ["indent", "Increase Indent", "ze_tin"],
    "removeformat": ["removeformat", "Remove Formatting", "ze_trf"],
    "image": ["image", "Insert Image", "ze_tim"],
    "smiley": ["smiley", "Insert smiley", "ze_tis"],
    "link": ["link", "Insert Link", "ze_til"],
    "unlink": ["unlink", "Remove Link", "ze_tuli"],
    "insertoptions": ["insertoptions", "Insert Options", "ze_spo"],
    "code": ["code", "Insert Code", "ze_icode"],//No I18N
    "quote": ["quote", "Insert Quote", "ze_quote"],//No I18N
    "edithtml": ["edithtml", "Edit HTML", "ze_teh"],
    /*"spellcheck":["spellcheck","Check Spelling","ze_sp"],
    "spellcheckoptions":["spellcheckoptions","Spell Check Languages","ze_spo"],
    "table":["table","Insert Table","ze_tbl"],
    "inserthorizontalrule":["inserthorizontalrule","Insert Horizontal Rule","ze_hr"],
    "object":["object","Insert HTML","ze_obj"],*/
};

ZE_Init.toolbarOrderGenerate = function (obj) {
    var toolbarArray = [],
    toolbarInnerArray,
    innerObj,
    key,
    innerKey;

    for (key in obj) {
        if (obj.hasOwnProperty(key)) {
            innerObj = obj[key];
            toolbarInnerArray = [];
            for (innerKey in innerObj) {
                if (innerObj.hasOwnProperty(innerKey)) {
                    toolbarInnerArray.push(ZE_Init.toolbarOrderMapping[innerObj[innerKey]]);
                }
            }
            toolbarArray.push(toolbarInnerArray);
        }
    }
    return toolbarArray;
};

ZE_Init.insertOptionsGenerate = function (array) {
    var options_array = [],
    array_length = array.length,
    i = 0;

    for (; i < array_length; i++) {
        options_array[i] = ZE_Init.toolbarOrderMapping[array[i]];
    }
    return options_array;
};