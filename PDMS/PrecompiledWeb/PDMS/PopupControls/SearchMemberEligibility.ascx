<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SearchMemberEligibility, App_Web_c4une0e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>

<script src="../Scripts/jquery.loadTemplate.min.js" type="text/javascript"></script>

<link href="../Content/custom-style.css" rel="stylesheet" />
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<link href="<%# Page.ResolveClientUrl("~/App_Themes/Modernization/Modern.css") %>" rel="stylesheet" />

<style>
    .claimTitle {
        display: flex;
        background: #2197bb;
        padding: 10px;
        font-size: 32px;
        align-items: center;
        width: 100%;
        justify-content: space-between
    }

        .claimTitle span {
            font-size: 40px;
            line-height: 1;
            color: #fff
        }

    .cttile {
        display: flex
    }

    .claimTitle h3 {
        margin: 0px;
        padding-left: 8px;
        color: #fff;
        padding-top: 4px;
    }

    .claimTitle .plus {
        display: none
    }

    .claimTitle.active .minus {
        display: none
    }

    .claimTitle.active .plus {
        display: block
    }

    .cForm input, .cForm select {
        height: 44px;
        line-height: 1;
        font-size: 17px;
        background-color: #FFF;
    }

    .dark-input {
        background-color: #d3d3d3; /* Dark background color */
        border: 1px solid #555; /* Border color */
        padding: 10px;
        width: 100%;
        box-sizing: border-box;
    }

    .maxclaim {
        display: flex;
        align-items: center;
        padding-top: 24px;
    }

    .mebtn {
        width: 180px;
        font-size: 20px;
        background-color: #435363;
        /* color: #ffffff*/
    }

    .clearme {
        width: 180px;
        font-size: 21px;
        background-color: #ccc;
    }

    .printme {
        width: 180px;
        font-size: 21px;
        background-color: #ccc;
    }

    .csresult1 .plus {
        display: block !important
    }

    .csresult1 .minus {
        display: none !important
    }

    .csresult1.active .minus {
        display: block !important
    }

    .csresult1.active .plus {
        display: none !important
    }

    .csTable thead tr {
        background: #435363;
        color: #ffffff;
    }

    .csTable tbody tr {
        background: #fff
    }

    .csresult2 .plus {
        display: block !important
    }

    .csresult2 .minus {
        display: none !important
    }

    .csresult2.active .minus {
        display: block !important
    }

    .csresult2.active .plus {
        display: none !important
    }

    .csresult3 .plus {
        display: block !important
    }

    .csresult3 .minus {
        display: none !important
    }

    .csresult3.active .minus {
        display: block !important
    }

    .csresult3.active .plus {
        display: none !important
    }

    .csresult4 .plus {
        display: block !important
    }

    .csresult4 .minus {
        display: none !important
    }

    .csresult4.active .minus {
        display: block !important
    }

    .csresult4.active .plus {
        display: none !important
    }

    .csresult5 .plus {
        display: block !important
    }

    .csresult5 .minus {
        display: none !important
    }

    .csresult5.active .minus {
        display: block !important
    }

    .csresult5.active .plus {
        display: none !important
    }

    .csresult6 .plus {
        display: block !important
    }

    .csresult6 .minus {
        display: none !important
    }

    .csresult6.active .minus {
        display: block !important
    }

    .csresult6.active .plus {
        display: none !important
    }

    .csresult7 .plus {
        display: block !important
    }

    .csresult7 .minus {
        display: none !important
    }

    .csresult7.active .minus {
        display: block !important
    }

    .csresult7.active .plus {
        display: none !important
    }

    .csresult8 .plus {
        display: block !important
    }

    .csresult8 .minus {
        display: none !important
    }

    .csresult8.active .minus {
        display: block !important
    }

    .csresult8.active .plus {
        display: none !important
    }

    .csresult9 .plus {
        display: block !important
    }

    .csresult9 .minus {
        display: none !important
    }

    .csresult9.active .minus {
        display: block !important
    }

    .csresult9.active .plus {
        display: none !important
    }

    .csresult10 .plus {
        display: block !important
    }

    .csresult10 .minus {
        display: none !important
    }

    .csresult10.active .minus {
        display: block !important
    }

    .csresult10.active .plus {
        display: none !important
    }

    .csresult11 .plus {
        display: block !important
    }

    .csresult11 .minus {
        display: none !important
    }

    .csresult11.active .minus {
        display: block !important
    }

    .csresult11.active .plus {
        display: none !important
    }

    .csresult12 .plus {
        display: block !important
    }

    .csresult12 .minus {
        display: none !important
    }

    .csresult12.active .minus {
        display: block !important
    }

    .csresult12.active .plus {
        display: none !important
    }

    .csresult13 .plus {
        display: block !important
    }

    .csresult13 .minus {
        display: none !important
    }

    .csresult13.active .minus {
        display: block !important
    }

    .csresult13.active .plus {
        display: none !important
    }
</style>

<style>
    .cursor {
        cursor: pointer;
    }

    .sort-icon3 {
        margin-left: 5px;
    }

    .sort-icon4 {
        margin-left: 5px;
    }

    .sort-icon6 {
        margin-left: 5px;
    }

    .sort-icon9 {
        margin-left: 5px;
    }

    .pagination3 {
        display: flex;
        list-style: none;
        padding: 0;
    }

        .pagination3 li {
            margin: 5px;
            cursor: pointer;
        }

            .pagination3 li.disabled {
                color: gray;
                cursor: not-allowed;
            }


    #pagination3 {
        text-align: right
    }

        #pagination3 .mybtnpage3 {
            font-size: 20px;
            padding: 9px 19px;
            background: #fff;
            border: 1px solid #d0c8c8;
        }

            #pagination3 .mybtnpage3:hover, #pagination3 .mybtnpage3[style] {
                color: #187ed5;
                background-color:darkgrey;
            }

        #pagination3 #btnPrev3, #pagination3 #btnNext3 {
            font-size: 20px;
            padding: 9px 18px;
            background: #fff;
            border: 1px solid #d0c8c8;
            width: 100px;
        }

            #pagination3 #btnPrev3:hover, #pagination3 #btnNext3:hover {
                color: #187ed5
            }

    .pagination4 {
        display: flex;
        list-style: none;
        padding: 0;
    }

        .pagination4 li {
            margin: 5px;
            cursor: pointer;
        }

            .pagination4 li.disabled {
                color: gray;
                cursor: not-allowed;
            }


    #pagination4 {
        text-align: right
    }

        #pagination4 .mybtnpage4 {
            font-size: 20px;
            padding: 9px 19px;
            background: #fff;
            border: 1px solid #d0c8c8;
        }

            #pagination4 .mybtnpage4:hover, #pagination4 .mybtnpage4[style] {
                color: #187ed5;
                background-color:darkgrey;
            }

        #pagination4 #btnPrev4, #pagination4 #btnNext4 {
            font-size: 20px;
            padding: 9px 18px;
            background: #fff;
            border: 1px solid #d0c8c8;
            width: 100px;
        }

            #pagination4 #btnPrev4:hover, #pagination4 #btnNext4:hover {
                color: #187ed5
            }

    .pagination5 {
        display: flex;
        list-style: none;
        padding: 0;
    }

        .pagination5 li {
            margin: 5px;
            cursor: pointer;
        }

            .pagination5 li.disabled {
                color: gray;
                cursor: not-allowed;
            }


    #pagination5 {
        text-align: right
    }

        #pagination5 .mybtnpage5 {
            font-size: 20px;
            padding: 9px 19px;
            background: #fff;
            border: 1px solid #d0c8c8;
        }

            #pagination5 .mybtnpage5:hover, #pagination5 .mybtnpage5[style] {
                color: #187ed5;
                background-color:darkgrey;
            }

        #pagination5 #btnPrev5, #pagination5 #btnNext5 {
            font-size: 20px;
            padding: 9px 18px;
            background: #fff;
            border: 1px solid #d0c8c8;
            width: 100px;
        }

            #pagination5 #btnPrev5:hover, #pagination5 #btnNext5:hover {
                color: #187ed5
            }

    .pagination6 {
        display: flex;
        list-style: none;
        padding: 0;
    }

        .pagination6 li {
            margin: 5px;
            cursor: pointer;
        }

            .pagination6 li.disabled {
                color: gray;
                cursor: not-allowed;
            }


    #pagination6 {
        text-align: right
    }

        #pagination6 .mybtnpage6 {
            font-size: 20px;
            padding: 9px 19px;
            background: #fff;
            border: 1px solid #d0c8c8;
        }

            #pagination6 .mybtnpage6:hover, #pagination6 .mybtnpage6[style] {
                color: #187ed5;
                background-color:darkgrey;
            }

        #pagination6 #btnPrev6, #pagination6 #btnNext6 {
            font-size: 20px;
            padding: 9px 18px;
            background: #fff;
            border: 1px solid #d0c8c8;
            width: 100px;
        }

            #pagination6 #btnPrev6:hover, #pagination6 #btnNext6:hover {
                color: #187ed5
            }

    .pagination7 {
        display: flex;
        list-style: none;
        padding: 0;
    }

        .pagination7 li {
            margin: 5px;
            cursor: pointer;
        }

            .pagination7 li.disabled {
                color: gray;
                cursor: not-allowed;
            }


    #pagination7 {
        text-align: right
    }

        #pagination7 .mybtnpage7 {
            font-size: 20px;
            padding: 9px 19px;
            background: #fff;
            border: 1px solid #d0c8c8;
        }

            #pagination7 .mybtnpage7:hover, #pagination7 .mybtnpage7[style] {
                color: #187ed5;
                background-color:darkgrey;
            }

        #pagination7 #btnPrev7, #pagination7 #btnNext7 {
            font-size: 20px;
            padding: 9px 18px;
            background: #fff;
            border: 1px solid #d0c8c8;
            width: 100px;
        }

            #pagination7 #btnPrev7:hover, #pagination7 #btnNext7:hover {
                color: #187ed5
            }

    .pagination8 {
        display: flex;
        list-style: none;
        padding: 0;
    }

        .pagination8 li {
            margin: 5px;
            cursor: pointer;
        }

            .pagination8 li.disabled {
                color: gray;
                cursor: not-allowed;
            }


    #pagination8 {
        text-align: right
    }

        #pagination8 .mybtnpage8 {
            font-size: 20px;
            padding: 9px 19px;
            background: #fff;
            border: 1px solid #d0c8c8;
        }

            #pagination8 .mybtnpage8:hover, #pagination8 .mybtnpage8[style] {
                color: #187ed5;
                background-color:darkgrey;
            }

        #pagination8 #btnPrev8, #pagination8 #btnNext8 {
            font-size: 20px;
            padding: 9px 18px;
            background: #fff;
            border: 1px solid #d0c8c8;
            width: 100px;
        }

            #pagination8 #btnPrev8:hover, #pagination8 #btnNext8:hover {
                color: #187ed5
            }

    .pagination9 {
        display: flex;
        list-style: none;
        padding: 0;
    }

        .pagination9 li {
            margin: 5px;
            cursor: pointer;
        }

            .pagination9 li.disabled {
                color: gray;
                cursor: not-allowed;
            }


    #pagination9 {
        text-align: right
    }

        #pagination9 .mybtnpage9 {
            font-size: 20px;
            padding: 9px 19px;
            background: #fff;
            border: 1px solid #d0c8c8;
        }

            #pagination9 .mybtnpage9:hover, #pagination9 .mybtnpage9[style] {
                color: #187ed5;
                background-color:darkgrey;
            }

        #pagination9 #btnPrev9, #pagination9 #btnNext9 {
            font-size: 20px;
            padding: 9px 18px;
            background: #fff;
            border: 1px solid #d0c8c8;
            width: 100px;
        }

            #pagination9 #btnPrev9:hover, #pagination9 #btnNext9:hover {
                color: #187ed5
            }

    .pagination10 {
        display: flex;
        list-style: none;
        padding: 0;
    }

        .pagination10 li {
            margin: 5px;
            cursor: pointer;
        }

            .pagination10 li.disabled {
                color: gray;
                cursor: not-allowed;
            }


    #pagination10 {
        text-align: right
    }

        #pagination10 .mybtnpage10 {
            font-size: 20px;
            padding: 9px 19px;
            background: #fff;
            border: 1px solid #d0c8c8;
        }

            #pagination10 .mybtnpage10:hover, #pagination10 .mybtnpage10[style] {
                color: #187ed5;
                background-color:darkgrey;
            }

        #pagination10 #btnPrev10, #pagination10 #btnNext10 {
            font-size: 20px;
            padding: 9px 18px;
            background: #fff;
            border: 1px solid #d0c8c8;
            width: 100px;
        }

            #pagination10 #btnPrev10:hover, #pagination10 #btnNext10:hover {
                color: #187ed5
            }

    .pagination11 {
        display: flex;
        list-style: none;
        padding: 0;
    }

        .pagination11 li {
            margin: 5px;
            cursor: pointer;
        }

            .pagination11 li.disabled {
                color: gray;
                cursor: not-allowed;
            }


    #pagination11 {
        text-align: right
    }

        #pagination11 .mybtnpage11 {
            font-size: 20px;
            padding: 9px 19px;
            background: #fff;
            border: 1px solid #d0c8c8;
        }

            #pagination11 .mybtnpage11:hover, #pagination11 .mybtnpage11[style] {
                color: #187ed5;
                background-color:darkgrey;
            }

        #pagination11 #btnPrev11, #pagination11 #btnNext11 {
            font-size: 20px;
            padding: 9px 18px;
            background: #fff;
            border: 1px solid #d0c8c8;
            width: 100px;
        }

            #pagination11 #btnPrev11:hover, #pagination11 #btnNext11:hover {
                color: #187ed5
            }

    .pagination12 {
        display: flex;
        list-style: none;
        padding: 0;
    }

        .pagination12 li {
            margin: 5px;
            cursor: pointer;
        }

            .pagination12 li.disabled {
                color: gray;
                cursor: not-allowed;
            }


    #pagination12 {
        text-align: right
    }

        #pagination12 .mybtnpage12 {
            font-size: 20px;
            padding: 9px 19px;
            background: #fff;
            border: 1px solid #d0c8c8;
        }

            #pagination12 .mybtnpage12:hover, #pagination12 .mybtnpage12[style] {
                color: #187ed5;
                background-color:darkgrey;
            }

        #pagination12 #btnPrev12, #pagination12 #btnNext12 {
            font-size: 20px;
            padding: 9px 18px;
            background: #fff;
            border: 1px solid #d0c8c8;
            width: 100px;
        }

            #pagination12 #btnPrev12:hover, #pagination12 #btnNext12:hover {
                color: #187ed5
            }

    .pagination13 {
        display: flex;
        list-style: none;
        padding: 0;
    }

        .pagination13 li {
            margin: 5px;
            cursor: pointer;
        }

            .pagination13 li.disabled {
                color: gray;
                cursor: not-allowed;
            }


    #pagination13 {
        text-align: right
    }

        #pagination13 .mybtnpage13 {
            font-size: 20px;
            padding: 9px 19px;
            background: #fff;
            border: 1px solid #d0c8c8;
        }

            #pagination13 .mybtnpage13:hover, #pagination13 .mybtnpage13[style] {
                color: #187ed5;
                background-color:darkgrey;
            }

        #pagination13 #btnPrev13, #pagination13 #btnNext13 {
            font-size: 20px;
            padding: 9px 18px;
            background: #fff;
            border: 1px solid #d0c8c8;
            width: 100px;
        }

            #pagination13 #btnPrev13:hover, #pagination13 #btnNext13:hover {
                color: #187ed5;

            }

    #divpnlLoader {
        text-align: center
    }

    .csTable th {
        border: 1px solid #f9f9f9
    }

    table.csTable td {
        border: 1px solid #e5e5e5 !important
    }

    table.csTable tr:nth-child(even) {
        background: #f2f9fd !important
    }

    .errMsgInput {
        border: 1px solid #f00 !important
    }

    .errMsg {
        color: #f00;
        position: absolute;
        display: block;
        text-align: right;
        width: 95%;
    }

    select {
        min-width: inherit !important
    }
</style>

<style type="text/css">
    .btn-primary {
        color: #ffffff;
        background-color: #004FA2;
        border-color: #004FA2;
        background-color: #337ab7;
        border-color: #337ab7;
    }

    .btn-default {
        background-color: #e6e6e6;
        border-color: #ccc;
    }

    label {
        display: inline-block;
        margin-bottom: 5px;
        font-weight: bold;
    }

    fieldset {
        display: block;
        min-inline-size: min-content;
        margin-inline: 2px;
        border-width: 2px;
        border-style: groove;
        border-color: threedface;
        border-image: initial;
        padding-block: 0.35em 0.625em;
        padding-inline: 0.75em;
    }

    .form-control {
        display: block;
        width: 100%;
        height: 34px;
        padding: 6px 12px;
        font-size: 14px;
        line-height: 1.428571429;
        color: #555555;
        vertical-align: middle;
        background-color: #ffffff;
        border: 1px solid #cccccc;
        border-radius: 4px;
        -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
        box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
        -webkit-transition: border-color ease-in-out 0.15s, box-shadow ease-in-out 0.15s;
        transition: border-color ease-in-out 0.15s, box-shadow ease-in-out 0.15s;
    }

    .form-horizontal .control-label, .form-horizontal .radio, .form-horizontal .checkbox, .form-horizontal .radio-inline, .form-horizontal .checkbox-inline {
        padding-top: 7px;
        margin-top: 0;
        margin-bottom: 0;
    }

    .collapsible {
        background-color: #2297bc;
        color: white;
        cursor: pointer;
        padding: 18px;
        width: 100%;
        border: none;
        text-align: left;
        outline: none;
    }

        .active, .collapsible:hover {
            background-color: #2297bc;
        }

    .content {
        padding: 10px 18px 10px 0px;
        display: block;
        overflow: hidden;
        background-color: #f1f1f1;
    }

    .hfont {
        color: white;
        font-size: 25px;
        font-weight: bold;
        padding-bottom: 5px;
        padding-top: 5px;
    }
</style>

<style type="text/css">
    .gridViewHeader a, .gridViewHeader a:link, .gridViewHeader a:active, .gridViewHeader a:hover, .gridViewHeader a:visited,
    .gridViewHeader th a, .gridViewHeader th a:link, .gridViewHeader th a:active, .gridViewHeader th a:hover, .gridViewHeader th a:visited,
    .rgHeader a, .rgHeader a:link, .rgHeader a:active, .rgHeader a:hover, .rgHeader a:visited,
    .rgHeader th a, .rgHeader th a:link, .rgHeader th a:active, .rgHeader th a:hover, .rgHeader th a:visited {
        color: #222222 !important;
        text-align: left;
    }

    .paging-nav {
        text-align: right;
        padding-top: 2px;
    }

        .paging-nav a {
            margin: auto 1px;
            text-decoration: none;
            display: inline-block;
            padding: 1px 7px;
            background: #91b9e6;
            color: white;
            border-radius: 3px;
        }

        .paging-nav .selected-page {
            background: #187ed5;
            font-weight: bold;
        }

    .paging-nav,
    #tableData {
        width: 400px;
        margin: 0 auto;
        font-family: Arial, sans-serif;
    }

    .cformContainer .cForm {
        padding: 20px
    }

    .cformContainer, .resultContainer1 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer.active, .resultContainer1.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .cformContainer, .resultContainer2 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer.active, .resultContainer2.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .cformContainer, .resultContainer3 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer.active, .resultContainer3.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .cformContainer, .resultContainer4 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer.active, .resultContainer4.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .cformContainer, .resultContainer5 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer.active, .resultContainer5.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .cformContainer, .resultContainer6 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer.active, .resultContainer6.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .cformContainer, .resultContainer7 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer.active, .resultContainer7.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .cformContainer, .resultContainer8 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer.active, .resultContainer8.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .cformContainer, .resultContainer9 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer.active, .resultContainer9.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .cformContainer, .resultContainer10 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer.active, .resultContainer10.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .cformContainer, .resultContainer11 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer.active, .resultContainer11.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .cformContainer, .resultContainer12 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer.active, .resultContainer12.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    .cformContainer, .resultContainer13 {
        height: 0px;
        transition: all 400ms ease-in-out;
        padding: 0;
        border: 1px solid #ccc;
        overflow: hidden;
    }

        .cformContainer.active, .resultContainer13.active {
            height: 100%;
            overflow: hidden;
            transition: all 400ms ease-in-out
        }

    input[type=number]::-webkit-inner-spin-button,
    input[type=number]::-webkit-outer-spin-button {
        -webkit-appearance: none;
        -moz-appearance: none;
        appearance: none;
        margin: 0;
    }

    .hidden {
        display: none;
    }

    .icon-rtl {
      background: url("../Images/Calendaricon.png") no-repeat right;
      background-size: 20px;
      background-origin: content-box;
    }
</style>

<script type="text/javascript">
    let currentPage3 = 1;
    let currentPage4 = 1;
    let currentPage5 = 1;
    let currentPage6 = 1;
    let currentPage7 = 1;
    let currentPage8 = 1;
    let currentPage9 = 1;
    let currentPage10 = 1;
    let currentPage11 = 1;
    let currentPage12 = 1;
    let currentPage13 = 1;

    let MEPanelLet1;
    let MEPanelLet2;
    let MEPanelLet3;
    let MEPanelLet4;
    let MEPanelLet5;
    let MEPanelLet6;
    let MEPanelLet7;
    let MEPanelLet8;
    let MEPanelLet9;
    
    let MEPanelLet11;
    let MEPanelLet12;
    let MEPanelLet13;
    const pageSize = 10;

</script>

<script type="text/javascript">
    function formatDatewithZero(input) {
        let parts = input.value.split(/[\/\-]/);
        if (parts.length === 3) {
            let day = parts[1].toString().padStart(2, '0');
            let month = parts[0].toString().padStart(2, '0');
            let year = parts[2];
            input.value = `${month}/${day}/${year}`;
        }
    }

    $(function () {
        $("#txtFDOS").datepicker();
    });
    $(function () {
        $("#txtTDOS").datepicker();
    });
    $(function () {
        $("#txtDOB").datepicker();
    });

    // Validates that the input string is a valid date formatted as "mm/dd/yyyy"
    function isValidDate(dateString) {
        // First check for the pattern
        if (!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(dateString))
            return false;

        // Parse the date parts to integers
        var parts = dateString.split("/");
        var day = parseInt(parts[1], 10);
        var month = parseInt(parts[0], 10);
        var year = parseInt(parts[2], 10);

        // Check the ranges of month and year
        if (year < 1000 || year > 3000 || month == 0 || month > 12)
            return false;

        var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        // Adjust for leap years
        if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
            monthLength[1] = 29;

        // Check the range of the day
        return day > 0 && day <= monthLength[month - 1];
    };

    function getFormattedDate(date) {
        var d = new Date(date + 'T00:00'),
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();
            if (month.length < 2)
                month = '0' + month;
            if (day.length < 2)
                day = '0' + day;

        return [month, day, year].join('/');
    }

    function makePrintPageRequest() {
        var APIToken = $("[id*=hdnAccessToken]").val();
        if (MEPanelLet1 == null) {
            MEPanelLet1 = [];
        }
        if (MEPanelLet2 == null) {
            MEPanelLet2 = [];
        }
        if (MEPanelLet3 == null) {
            MEPanelLet3 = [];
        }
        if (MEPanelLet4 == null) {
            MEPanelLet4 = [];
        }
        if (MEPanelLet5 == null) {
            MEPanelLet5 = [];
        }
        if (MEPanelLet6 == null) {
            MEPanelLet6 = [];
        }
        if (MEPanelLet7 == null) {
            MEPanelLet7 = [];
        }
        if (MEPanelLet8 == null) {
            MEPanelLet8 = [];
        }
        if (MEPanelLet9 == null) {
            MEPanelLet9 = [];
        }
        if (MEPanelLet11 == null) {
            MEPanelLet11 = [];
        }
        if (MEPanelLet12 == null) {
            MEPanelLet12 = [];
        }
        if (MEPanelLet13 == null) {
            MEPanelLet13 = [];
        }
        var meRequestDetails2 = {
            RecipientSearchParameters: MEPanelLet1,
            RecipientInfo: MEPanelLet2,
            BenifitAssignmentPlans: MEPanelLet3,
            ManagedCarePlans: MEPanelLet4,
            ThirdPartyLiabilities: MEPanelLet5,
            PatientLiabilities: MEPanelLet6,
            LTCFPlacements: MEPanelLet7,
            Lockins: MEPanelLet8,
            MedicareCoverageDetails: MEPanelLet9,
            ServiceLimitations: MEPanelLet11,
            RestrictedCoverages: MEPanelLet12,
            Under19FamilyMembers: MEPanelLet13
        }
        var inputData2 = JSON.stringify(meRequestDetails2);
        
        $.ajax({
            type: "POST",
            url: webApiPrint + "GenerateMemberEligibilityPdfJSON",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: inputData2,
            complete: function (result) {
                if (result != null) {
                    var obj = JSON.parse(result.responseText);
                    if (obj != null && obj.response == 'SUCCESS') {
                        var binaryImg = atob(obj.fileBytes);
                        var length = binaryImg.length;
                        var arrayBuffer = new ArrayBuffer(length);
                        var uintArray = new Uint8Array(arrayBuffer);

                        for (let i = 0; i < length; i++) {
                            uintArray[i] = binaryImg.charCodeAt(i);
                        }

                        var fileBlob = new Blob([uintArray], { type: 'application/pdf' });
                        var url = window.URL.createObjectURL(fileBlob);
                        window.open(url, "Title", "").print();
                    } else {
                        document.getElementById('pnlMEResults').innerHTML = '';
                        document.getElementById('pnlMEResults').innerHTML = '<span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">Error generating the print.</span>';
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var status = jqXHR.status;
                document.getElementById('pnlMEResults').innerHTML = '';
                document.getElementById('pnlMEResults').innerHTML = '<span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">Error : ' + status + ' No data returned</span>';
                console.log(JSON.stringify(jqXHR));
            }
        });
    }

    function clearForm() {
        document.getElementById('txtMBN').value = '';
        document.getElementById('txtSSN').value = '';
        document.getElementById('txtDOB').value = '';
        document.getElementById('txtFDOS').value = '';
        document.getElementById('txtTDOS').value = '';
        document.getElementById('txtProcCode').value = '';

        document.getElementById('txtMBNDisp').value = '';
        document.getElementById('txtLNDisp').value = '';
        document.getElementById('txtFNLNDisp').value = '';
        document.getElementById('txtDOBDisp').value = '';
        document.getElementById('txtDODDisp').value = '';
        document.getElementById('txtSSNDisp').value = '';
        document.getElementById('txtGenderDisp').value = '';
        document.getElementById('txtCORDisp').value = '';
        document.getElementById('txtCOEDisp').value = '';

        document.getElementById('spantxtMBN').innerHTML = '';
        document.getElementById('spantxtSSN').innerHTML = '';
        document.getElementById('spantxtDOB').innerHTML = '';
        document.getElementById('spantxtFDOS').innerHTML = '';
        document.getElementById('spantxtTDOS').innerHTML = '';
        document.getElementById('spantxtProcCode').innerHTML = '';


        document.getElementById('pnlResults3').innerHTML = '';
        document.getElementById('pnlResults4').innerHTML = '';
        document.getElementById('pnlResults5').innerHTML = '';
        document.getElementById('pnlResults6').innerHTML = '';
        document.getElementById('pnlResults7').innerHTML = '';
        document.getElementById('pnlResults8').innerHTML = '';
        document.getElementById('pnlResults9').innerHTML = '';
        
        document.getElementById('pnlResults11').innerHTML = '';
        document.getElementById('pnlResults12').innerHTML = '';
        document.getElementById('pnlResults13').innerHTML = '';

        document.getElementById('pagination3').innerHTML = '';
        document.getElementById('pagination4').innerHTML = '';
        document.getElementById('pagination5').innerHTML = '';
        document.getElementById('pagination6').innerHTML = '';
        document.getElementById('pagination7').innerHTML = '';
        document.getElementById('pagination8').innerHTML = '';
        document.getElementById('pagination9').innerHTML = '';
        
        document.getElementById('pagination11').innerHTML = '';
        document.getElementById('pagination12').innerHTML = '';
        document.getElementById('pagination13').innerHTML = '';

        document.getElementById('txtMBN').classList.remove('errMsgInput');
        document.getElementById('txtSSN').classList.remove('errMsgInput');
        document.getElementById('txtDOB').classList.remove('errMsgInput');
        document.getElementById('txtFDOS').classList.remove('errMsgInput');
        document.getElementById('txtTDOS').classList.remove('errMsgInput');
        document.getElementById('txtProcCode').classList.remove('errMsgInput');

        document.getElementById('pnlMEResults').innerHTML = '';

        $('.resultContainer2').removeClass("active");
        $('.resultContainer3').removeClass("active");
        $('.resultContainer4').removeClass("active");
        $('.resultContainer5').removeClass("active");
        $('.resultContainer6').removeClass("active");
        $('.resultContainer7').removeClass("active");
        $('.resultContainer8').removeClass("active");
        $('.resultContainer9').removeClass("active");
        
        $('.resultContainer11').removeClass("active");
        $('.resultContainer12').removeClass("active");
        $('.resultContainer13').removeClass("active");

        $('.csresult2').removeClass("active");
        $('.csresult3').removeClass("active");
        $('.csresult4').removeClass("active");
        $('.csresult5').removeClass("active");
        $('.csresult6').removeClass("active");
        $('.csresult7').removeClass("active");
        $('.csresult8').removeClass("active");
        $('.csresult9').removeClass("active");
        
        $('.csresult11').removeClass("active");
        $('.csresult12').removeClass("active");
        $('.csresult13').removeClass("active");

        document.getElementById('btnPrint').style.display = 'none';

        document.getElementById("btnSearch").disabled = false;

        document.getElementById('paneltbl3').innerHTML = '';
        document.getElementById('paneltbl4').innerHTML = '';
        document.getElementById('paneltbl5').innerHTML = '';
        document.getElementById('paneltbl6').innerHTML = '';
        document.getElementById('paneltbl12').innerHTML = '';
        document.getElementById('paneltbl8').innerHTML = '';
        document.getElementById('paneltbl9').innerHTML = '';
        document.getElementById('paneltbl11').innerHTML = '';
        document.getElementById('paneltbl12').innerHTML = '';
        document.getElementById('paneltbl13').innerHTML = '';

        MEPanelLet1 = null;
        MEPanelLet2 = null;
        MEPanelLet3 = null;
        MEPanelLet4 = null;
        MEPanelLet5 = null;
        MEPanelLet6 = null;
        MEPanelLet7 = null;
        MEPanelLet8 = null;
        MEPanelLet9 = null;
        MEPanelLet11 = null;
        MEPanelLet12 = null;
        MEPanelLet13 = null;
    }

    function clearErrorMessages() {
        document.getElementById('spantxtMBN').innerHTML = '';
        document.getElementById('spantxtSSN').innerHTML = '';
        document.getElementById('spantxtDOB').innerHTML = '';
        document.getElementById('spantxtFDOS').innerHTML = '';
        document.getElementById('spantxtTDOS').innerHTML = '';
        document.getElementById('spantxtProcCode').innerHTML = '';

        document.getElementById('txtMBN').classList.remove('errMsgInput');
        document.getElementById('txtSSN').classList.remove('errMsgInput');
        document.getElementById('txtDOB').classList.remove('errMsgInput');
        document.getElementById('txtFDOS').classList.remove('errMsgInput');
        document.getElementById('txtTDOS').classList.remove('errMsgInput');
        document.getElementById('txtProcCode').classList.remove('errMsgInput');

        document.getElementById('pnlMEResults').innerHTML = '';
    }

    function NowDate() {
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = String(today.getMonth() + 1).padStart(2, '0');
        var yyyy = today.getFullYear();
        today = mm + '/' + dd + '/' + yyyy;
        return today;
    }

    function validateFields() {
        clearErrorMessages();
        var message = '';
        var txtMBNVal = document.getElementById('txtMBN').value;
        var txtSSNVal = document.getElementById('txtSSN').value;
        var txtDOBVal = document.getElementById('txtDOB').value;
        var txtFDOSVal = document.getElementById('txtFDOS').value;
        var txtTDOSVal = document.getElementById('txtTDOS').value;
        var txtProcCodeVal = document.getElementById('txtProcCode').value;
        var valid = true;

        if (txtMBNVal == null || txtMBNVal == '') {
            if (txtSSNVal == null || txtSSNVal == '') {
                message = "* Please Enter Medicaid Billing Number or SSN.";
                document.getElementById('txtMBN').classList.add('errMsgInput');
                document.getElementById('txtSSN').classList.add('errMsgInput');
                document.getElementById('spantxtMBN').innerHTML = message;
                document.getElementById('spantxtSSN').innerHTML = message;
                valid = false;
            }
        }

        if (txtMBNVal != null && txtMBNVal != '') {
            if (txtMBNVal.length < 12 || txtMBNVal.length > 12) {
                message = "* 12-digit number is required.";
                document.getElementById('txtMBN').classList.add('errMsgInput');
                document.getElementById('spantxtMBN').innerHTML = message;
                valid = false;
            }
        }

        if (txtSSNVal != null && txtSSNVal != '') {
            if (txtSSNVal.length < 9 || txtSSNVal.length > 9) {
                message = "* 9-digits number required.";
                document.getElementById('txtSSN').classList.add('errMsgInput');
                document.getElementById('spantxtSSN').innerHTML = message;
                valid = false;
            }
        }

        if (txtProcCodeVal != null && txtProcCodeVal != '') {
            if (txtProcCodeVal.length > 20) {
                message = "* Maximum 20 characters allowed.";
                document.getElementById('txtProcCode').classList.add('errMsgInput');
                document.getElementById('spantxtProcCode').innerHTML = message;
                valid = false;
            }
        }

        if (txtDOBVal == null || txtDOBVal == '') {
            message = "* Please Enter Date Of Birth.";
            document.getElementById('txtDOB').classList.add('errMsgInput');
            document.getElementById('spantxtDOB').innerHTML = message;
            valid = false;
        }

        if (txtDOBVal != null && txtDOBVal != '') {
            if (new Date(txtDOBVal).valueOf() < new Date('01/01/1900').valueOf()) {
                message = "* DOB Must be beyond 1900.";
                document.getElementById('txtDOB').classList.add('errMsgInput');
                document.getElementById('spantxtDOB').innerHTML = message;
                valid = false;
            }

            var todaysDate = NowDate();
            if (new Date(txtDOBVal).valueOf() > new Date(todaysDate).valueOf()) {
                message = "* DOB Does not allow future date.";
                document.getElementById('txtDOB').classList.add('errMsgInput');
                document.getElementById('spantxtDOB').innerHTML = message;
                valid = false;
            }

            if (!isValidDate(txtDOBVal)) {
                message = "* DOB is not of valid format mm/dd/yyyy.";
                document.getElementById('txtDOB').classList.add('errMsgInput');
                document.getElementById('spantxtDOB').innerHTML = message;
                valid = false;
            }
        }

        if (txtFDOSVal == null || txtFDOSVal == '') {
            message = "* Please Select From Date of Service.";
            document.getElementById('txtFDOS').classList.add('errMsgInput');
            document.getElementById('spantxtFDOS').innerHTML = message;
            valid = false;
        }

        if (txtFDOSVal != null && txtFDOSVal != '') {

            if (!isValidDate(txtFDOSVal)) {
                message = "* From DOS is not of valid format mm/dd/yyyy.";
                document.getElementById('txtFDOS').classList.add('errMsgInput');
                document.getElementById('spantxtFDOS').innerHTML = message;
                valid = false;
            }

            if (txtTDOSVal != null && txtTDOSVal != '') {
                
                if (new Date(txtFDOSVal).valueOf() > new Date(txtTDOSVal).valueOf()) {
                    message = "* Choose DateFrom value prior to DateTo value";
                    document.getElementById('txtFDOS').classList.add('errMsgInput');
                    document.getElementById('spantxtFDOS').innerHTML = message;
                    valid = false;
                }
            }

            var todaysDate = NowDate();
            
            if (new Date(txtFDOSVal).valueOf() > new Date(todaysDate).valueOf()) {
                message = "* From DOS cannot be after todays date.";
                document.getElementById('txtFDOS').classList.add('errMsgInput');
                document.getElementById('spantxtFDOS').innerHTML = message;
                valid = false;
            }

            if (txtTDOSVal != null && txtTDOSVal != '') {
                if (txtFDOSVal != null && txtFDOSVal != '') {
                    var todaysDateSub = new Date();
                    var minYear = todaysDateSub.getFullYear() - 4;
                    todaysDateSub.setFullYear(minYear);
                    var sub48 = todaysDateSub.toISOString().substring(0, 10);
                    
                    if (new Date(txtFDOSVal).valueOf() < new Date(sub48).valueOf()) {
                        message = "* System Allows up to 48 months back.";
                        document.getElementById('txtFDOS').classList.add('errMsgInput');
                        document.getElementById('spantxtFDOS').innerHTML = message;
                        valid = false;
                    }
                }
            }
        }

        if (txtTDOSVal != null && txtTDOSVal != '') {

            if (!isValidDate(txtTDOSVal)) {
                message = "* To DOS is not of valid format mm/dd/yyyy.";
                document.getElementById('txtTDOS').classList.add('errMsgInput');
                document.getElementById('spantxtTDOS').innerHTML = message;
                valid = false;
            }

            if (txtFDOSVal != null && txtFDOSVal != '') {
                
                if (new Date(txtFDOSVal).valueOf() > new Date(txtTDOSVal).valueOf()) {
                    message = "* To DOS must be greater than From DOS";
                    document.getElementById('txtFDOS').classList.add('errMsgInput');
                    document.getElementById('spantxtFDOS').innerHTML = message;
                    valid = false;
                }
            }

            var todaysDate = NowDate();
            
            if (new Date(txtTDOSVal).valueOf() > new Date(todaysDate).valueOf()) {
                message = "* To DOS cannot be after todays Date.";
                document.getElementById('txtTDOS').classList.add('errMsgInput');
                document.getElementById('spantxtTDOS').innerHTML = message;
                valid = false;
            }
        }

        if (txtTDOSVal == null || txtTDOSVal == '') {
            message = "* Please Select To Date of Service.";
            document.getElementById('txtTDOS').classList.add('errMsgInput');
            document.getElementById('spantxtTDOS').innerHTML = message;
            valid = false;
        }
        return valid;
    }

    function clearPanels() {
        document.getElementById('btnPrint').style.display = 'none';

        $('.resultContainer2').removeClass("active");
        $('.resultContainer3').removeClass("active");
        $('.resultContainer4').removeClass("active");
        $('.resultContainer5').removeClass("active");
        $('.resultContainer6').removeClass("active");
        $('.resultContainer7').removeClass("active");
        $('.resultContainer8').removeClass("active");
        $('.resultContainer9').removeClass("active");
        
        $('.resultContainer11').removeClass("active");
        $('.resultContainer12').removeClass("active");
        $('.resultContainer13').removeClass("active");

        $('.csresult2').removeClass("active");
        $('.csresult3').removeClass("active");
        $('.csresult4').removeClass("active");
        $('.csresult5').removeClass("active");
        $('.csresult6').removeClass("active");
        $('.csresult7').removeClass("active");
        $('.csresult8').removeClass("active");
        $('.csresult9').removeClass("active");
        
        $('.csresult11').removeClass("active");
        $('.csresult12').removeClass("active");
        $('.csresult13').removeClass("active");
    }

    //Use this when the date is in 'mm/dd/yyyy' format to convert it to 'YYYY-MM-DD'
    function formatDate(date) {
        var da = date.split('/')
        //var d = new Date(date + 'EST'),
        //    month = '' + (d.getMonth() + 1),
        //    day = '' + (d.getDate() + 1),
        //    year = d.getFullYear();

        //if (month.length < 2)
        //    month = '0' + month;
        //if (day.length < 2)
        //    day = '0' + day;

        return [da[2], da[0], da[1]].join('-');
    }

    function makeMemberEligibilityPageRequest() {
        document.getElementById("btnSearch").disabled = true;
        clearPanels();
        if (validateFields() == false) {
            document.getElementById("btnSearch").disabled = false;
            return false;
        }

        var currentdate = new Date();
        document.getElementById('<%= hdnSearchDateTime.ClientID %>').value = currentdate.getFullYear() + "-" + ("00" + (currentdate.getMonth() + 1)).slice(-2) + "-" + ("00" + currentdate.getDate()).slice(-2) + " " + ("00" + currentdate.getHours()).slice(-2) + ":" + ("00" + currentdate.getMinutes()).slice(-2) + ":" + ("00" + currentdate.getSeconds()).slice(-2);

        var txtMBNVal = document.getElementById('txtMBN').value;
        var txtSSNVal = document.getElementById('txtSSN').value;
        var txtDOBVal = document.getElementById('txtDOB').value;
        var txtFDOSVal = document.getElementById('txtFDOS').value;
        var txtTDOSVal = document.getElementById('txtTDOS').value;
        var txtProcCodeVal = document.getElementById('txtProcCode').value;
        var username = $("[id*=hdnUserName]").val();
        var medID = $("[id*=hdnMedicaidID]").val();
        var searchDateTime = $("[id*=hdnSearchDateTime]").val();

        var meRequestDetails = {
            MEpid: medID,
            MEmbn: txtMBNVal,
            MEssn: txtSSNVal,
            MEdob: formatDate(txtDOBVal),
            MEfdos: formatDate(txtFDOSVal),
            MEtdos: formatDate(txtTDOSVal),
            MEpc: txtProcCodeVal,
            MErt: 'SearchEligibility',
            MEun: username,
            MEsdt: searchDateTime
        }
        MEPanelLet1 = meRequestDetails;
        var inputData = JSON.stringify(meRequestDetails);
        var APIToken = $("[id*=hdnAccessToken]").val();
        
        $.ajax({
            type: "POST",
            url: webApiME + "GetRecipientPageInformationV2",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: inputData,
            success: function (result) {
                if (result != null) {
                    document.getElementById("btnSearch").disabled = false;
                    var respHeader = result.ResponseHeaderDetails;
                    var respError = result.ErrorDetails;
                    if (respError != null && respError != '' && respError[0].Code != null && respError[0].Code != '') {
                        document.getElementById('btnPrint').style.display = 'none';
                        document.getElementById('pnlMEResults').innerHTML = '';
                        document.getElementById('pnlMEResults').innerHTML = '<span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">Error Code: ' + respError[0].Code + ', Error Description: ' + respError[0].Description + '</span>';
                    }
                    else if (respHeader != null && respHeader.ResponseType != null && respHeader.ResponseType != '' && respHeader.ResponseType == "FAILURE") {
                        document.getElementById('btnPrint').style.display = 'none';
                        var responseType = respHeader.ResponseType;
                        var ResponseMessage = respHeader.ResponseMessage;
                        var ResponseDetails = respHeader.ResponseDetails;
                        document.getElementById('pnlMEResults').innerHTML = '';
                        document.getElementById('pnlMEResults').innerHTML = '<span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">No data returned</span>';
                    }
                    else if (respHeader != null && respHeader.ResponseType != null && respHeader.ResponseType != '' && respHeader.ResponseType == "SUCCESS") {
                        document.getElementById("btnSearch").disabled = false;
                        var MERecipientInfo = result.RecipientInfo
                        if (MERecipientInfo != null) {
                            MEPanelLet2 = MERecipientInfo;
                            var MEMedicaidId = MERecipientInfo.MedicaidId;
                            var MEDateOfBirth = MERecipientInfo.DateOfBirth;
                            var MEDateOfDeath = MERecipientInfo.DateOfDeath;
                            var MEFirstName = MERecipientInfo.FirstName;
                            var MEMiddleName = MERecipientInfo.MiddleName;
                            var MELastName = MERecipientInfo.LastName;
                            var MESSN = MERecipientInfo.SSN;
                            var MEGender = MERecipientInfo.Gender;
                            var MECOR = MERecipientInfo.CountyOfResidence;
                            var MECOE = MERecipientInfo.CountyOfEligibility;

                            if (MEMedicaidId != null) {
                                document.getElementById('txtMBNDisp').value = '';
                                document.getElementById('txtMBNDisp').value = MEMedicaidId;
                            } else {
                                document.getElementById('txtMBNDisp').value = '';
                            }

                            if (MEDateOfBirth != null) {
                                document.getElementById('txtDOBDisp').value = '';
                                document.getElementById('txtDOBDisp').value = getFormattedDate(MEDateOfBirth.split('T')[0]);
                            } else {
                                document.getElementById('txtDOBDisp').value = '';
                            }

                            if (MEDateOfDeath != null) {
                                document.getElementById('txtDODDisp').value = '';
                                document.getElementById('txtDODDisp').value = getFormattedDate(MEDateOfDeath.split('T')[0]);
                            } else {
                                document.getElementById('txtDODDisp').value = '';
                            }

                            if (MEFirstName != null) {
                                document.getElementById('txtFNLNDisp').value = '';
                                if (MEMiddleName != null && MEMiddleName != '') {
                                    document.getElementById('txtFNLNDisp').value = MEFirstName + ', ' + MEMiddleName;
                                } else {
                                    document.getElementById('txtFNLNDisp').value = MEFirstName;
                                }
                            } else {
                                document.getElementById('txtFNLNDisp').value = '';
                            }

                            if (MELastName != null) {
                                document.getElementById('txtLNDisp').value = '';
                                document.getElementById('txtLNDisp').value = MELastName;
                            } else {
                                document.getElementById('txtLNDisp').value = '';
                            }

                            var ssn = document.getElementById("txtSSN").value;
                            if (MESSN != null && ssn == MESSN) {
                                document.getElementById('txtSSNDisp').value = '';
                                document.getElementById('txtSSNDisp').value = MESSN;
                            } else {
                                document.getElementById('txtSSNDisp').value = '';
                            }

                            if (MEGender != null) {
                                document.getElementById('txtGenderDisp').value = '';
                                document.getElementById('txtGenderDisp').value = MEGender;
                            } else {
                                document.getElementById('txtGenderDisp').value = '';
                            }

                            if (MEGender != null) {
                                document.getElementById('txtGenderDisp').value = '';
                                document.getElementById('txtGenderDisp').value = MEGender;
                            } else {
                                document.getElementById('txtGenderDisp').value = '';
                            }

                            if (MECOR != null) {
                                document.getElementById('txtCORDisp').value = '';
                                document.getElementById('txtCORDisp').value = MECOR;
                            } else {
                                document.getElementById('txtCORDisp').value = '';
                            }

                            if (MECOE != null) {
                                document.getElementById('txtCOEDisp').value = '';
                                document.getElementById('txtCOEDisp').value = MECOE;
                            } else {
                                document.getElementById('txtCOEDisp').value = '';
                            }

                            $('.resultContainer2').toggleClass("active");
                            $('.csresult2').toggleClass("active");
                        }

                        try {
                            var MEBenifitAssignmentPlans = result.BenifitAssignmentPlans;
                            if (MEBenifitAssignmentPlans != null && MEBenifitAssignmentPlans != '' && MEBenifitAssignmentPlans.length > 0) {
                                MEPanelLet3 = MEBenifitAssignmentPlans;
                                var MEBenifitAssignmentPlansSliced = MEPanelLet3.slice(0, pageSize);
                                var totalPages = Math.ceil(MEPanelLet3.length / pageSize);

                                var table = "<table id='paneltbl3' class='table csTable'><thead><tr><th class='cursor' onclick='sortTable3(0)'>Benefit/Assignment Plan <span id='icon-0' class='sort-icon3'>&#8597;</span></th><th class='cursor' onclick='sortTable3(1)'>Effective Date <span id='icon-1' class='sort-icon3'>&#8597;</span></th><th class='cursor' onclick='sortTable3(2)'>End Date <span id='icon-2' class='sort-icon3'>&#8597;</span></th></tr></thead><tbody>";
                                for (var i = 0; i < MEBenifitAssignmentPlansSliced.length; i++) {

                                    var BAAssignmentPlan = '';
                                    var BAEffectiveDate = '';
                                    var BAEndDate = '';

                                    if (MEBenifitAssignmentPlansSliced[i].AssignmentPlan != null) {
                                        BAAssignmentPlan = MEBenifitAssignmentPlansSliced[i].AssignmentPlan;
                                    }
                                    if (MEBenifitAssignmentPlansSliced[i].EffectiveDate != null) {
                                        BAEffectiveDate = getFormattedDate(MEBenifitAssignmentPlansSliced[i].EffectiveDate.split('T')[0]);
                                    }
                                    if (MEBenifitAssignmentPlansSliced[i].EndDate != null) {
                                        BAEndDate = getFormattedDate(MEBenifitAssignmentPlansSliced[i].EndDate.split('T')[0]);
                                    }

                                    table = table + "<tr><td>" + BAAssignmentPlan + "</td><td>" + BAEffectiveDate + "</td><td>" + BAEndDate + "</td></tr>";
                                }

                                table = table + "</tbody></table>";
                                document.getElementById('pnlResults3').innerHTML = '';
                                document.getElementById('pnlResults3').innerHTML = table;

                                if (MEPanelLet3.length > pageSize)
                                    setupPagination3(totalPages, 1);

                                $('.resultContainer3').toggleClass("active");
                                $('.csresult3').toggleClass("active");
                            }
                            else {
                                if (document.getElementById('paneltbl3') != null) {
                                    document.getElementById('paneltbl3').innerHTML = '';
                                }
                                MEBenifitAssignmentPlans = null;
                                MEPanelLet3 = null;
                            }
                        }
                        catch (err) {
                            console.log(err);
                        }

                        try {
                            var MEListOfManagedCarePlan = result.ManagedCarePlans
                            if (MEListOfManagedCarePlan != null && MEListOfManagedCarePlan != '' && MEListOfManagedCarePlan.length > 0) {
                                MEPanelLet4 = MEListOfManagedCarePlan;
                                var MEListOfManagedCarePlanSliced = MEPanelLet4.slice(0, pageSize);
                                var totalPages = Math.ceil(MEPanelLet4.length / pageSize);
                                var table = "<table id='paneltbl4' class='table csTable'><thead><tr><th class='cursor' onclick='sortTable4(0)'>Plan Name <span id='icon-0' class='sort-icon4'>&#8597;</span></th><th class='cursor' onclick='sortTable4(1)'>Payer ID <span id='icon-1' class='sort-icon4'>&#8597;</span></th><th>Plan Description</th><th class='cursor' onclick='sortTable4(3)'>Effective Date <span id='icon-3' class='sort-icon4'>&#8597;</span></th><th class='cursor' onclick='sortTable4(4)'>End Date <span id='icon-4' class='sort-icon4'>&#8597;</span></th><th>Managed Care Benefits</th></tr></thead><tbody>";
                                for (var i = 0; i < MEListOfManagedCarePlanSliced.length; i++) {
                                    var MEManagedCarePlanId = '';
                                    var MEPlanName = '';
                                    var MEPlanDescription = '';
                                    var MEEffectiveDate = '';
                                    var MEEndDate = '';
                                    var MEManagedCareBenefits = '';

                                    if (MEListOfManagedCarePlanSliced[i].PlanId != null) {
                                        MEManagedCarePlanId = MEListOfManagedCarePlanSliced[i].PlanId;
                                    }
                                    if (MEListOfManagedCarePlanSliced[i].PlanName != null) {
                                        MEPlanName = MEListOfManagedCarePlanSliced[i].PlanName;
                                    }
                                    if (MEListOfManagedCarePlanSliced[i].PlanDescription != null) {
                                        MEPlanDescription = MEListOfManagedCarePlanSliced[i].PlanDescription;
                                    }
                                    if (MEListOfManagedCarePlanSliced[i].EffectiveDate != null) {
                                        MEEffectiveDate = getFormattedDate(MEListOfManagedCarePlanSliced[i].EffectiveDate.split('T')[0]);
                                    }
                                    if (MEListOfManagedCarePlanSliced[i].EndDate != null) {
                                        MEEndDate = getFormattedDate(MEListOfManagedCarePlanSliced[i].EndDate.split('T')[0]);
                                    }
                                    if (MEListOfManagedCarePlanSliced[i].ManagedCareBenefits != null) {
                                        MEManagedCareBenefits = MEListOfManagedCarePlanSliced[i].ManagedCareBenefits;
                                    }

                                    table = table + "<tr><td>" + MEPlanName + "</td><td>" + MEManagedCarePlanId + "</td><td>" + MEPlanDescription + "</td><td>" + MEEffectiveDate + "</td><td>" + MEEndDate + "</td><td>" + MEManagedCareBenefits + "</td></tr>";
                                }

                                table = table + "</tbody></table>";
                                document.getElementById('pnlResults4').innerHTML = '';
                                document.getElementById('pnlResults4').innerHTML = table;

                                if (MEPanelLet4.length > pageSize)
                                    setupPagination4(totalPages, 1);

                                $('.resultContainer4').toggleClass("active");
                                $('.csresult4').toggleClass("active");
                            }
                            else {
                                if (document.getElementById('paneltbl4') != null) {
                                    document.getElementById('paneltbl4').innerHTML = '';
                                }
                                MEListOfManagedCarePlan = null;
                                MEPanelLet4 = null;
                            }
                        }
                        catch (err) {
                            console.log(err);
                        }

                        try {
                            var METhirdPartyLiabilities = result.ThirdPartyLiabilities
                            if (METhirdPartyLiabilities != null && METhirdPartyLiabilities != '' && METhirdPartyLiabilities.length > 0) {
                                MEPanelLet5 = METhirdPartyLiabilities;
                                var METhirdPartyLiabilitiesSliced = MEPanelLet5.slice(0, pageSize);
                                var totalPages = Math.ceil(MEPanelLet5.length / pageSize);
                                var table = "<table id='paneltbl5' class='table csTable'><thead><tr><th>Carrier Name</th><th>Carrier Number</th><th>NAIC</th><th>Policy Number</th><th>Policy Holder</th><th>Coverage Type</th><th>Coverage</th><th>Effective Date</th><th>End Date</th><th>Group Number</th></tr></thead><tbody>";
                                for (var i = 0; i < METhirdPartyLiabilitiesSliced.length; i++) {

                                    var MECarrierName = '';
                                    var MECarrierNumber = '';
                                    var MENAIC = '';
                                    var MEPolicyNumber = '';
                                    var MEPolicyHolder = '';
                                    var MECoverageType = '';
                                    var MECoverage = '';
                                    var MEEffectiveDate = '';
                                    var MEEndDate = '';
                                    var MEGroupNumber = '';

                                    if (METhirdPartyLiabilitiesSliced[i].CarrierName != null) {
                                        MECarrierName = METhirdPartyLiabilitiesSliced[i].CarrierName;
                                    }
                                    if (METhirdPartyLiabilitiesSliced[i].CarrierNumber != null) {
                                        MECarrierNumber = METhirdPartyLiabilitiesSliced[i].CarrierNumber;
                                    }
                                    if (METhirdPartyLiabilitiesSliced[i].NAIC != null) {
                                        MENAIC = METhirdPartyLiabilitiesSliced[i].NAIC;
                                    }
                                    if (METhirdPartyLiabilitiesSliced[i].PolicyNumber != null) {
                                        MEPolicyNumber = METhirdPartyLiabilitiesSliced[i].PolicyNumber;
                                    }
                                    if (METhirdPartyLiabilitiesSliced[i].PolicyHolder != null) {
                                        MEPolicyHolder = METhirdPartyLiabilitiesSliced[i].PolicyHolder;
                                    }
                                    if (METhirdPartyLiabilitiesSliced[i].CoverageType != null) {
                                        MECoverageType = METhirdPartyLiabilitiesSliced[i].CoverageType;
                                    }
                                    if (METhirdPartyLiabilitiesSliced[i].Coverage != null) {
                                        MECoverage = METhirdPartyLiabilitiesSliced[i].Coverage;
                                    }
                                    if (METhirdPartyLiabilitiesSliced[i].EffectiveDate != null) {
                                        MEEffectiveDate = getFormattedDate(METhirdPartyLiabilitiesSliced[i].EffectiveDate.split('T')[0]);
                                    }
                                    if (METhirdPartyLiabilitiesSliced[i].EndDate != null) {
                                        MEEndDate = getFormattedDate(METhirdPartyLiabilitiesSliced[i].EndDate.split('T')[0]);
                                    }
                                    if (METhirdPartyLiabilitiesSliced[i].GroupNumber != null) {
                                        MEGroupNumber = METhirdPartyLiabilitiesSliced[i].GroupNumber;
                                    }

                                    table = table + "<tr><td>" + MECarrierName + "</td><td>" + MECarrierNumber + "</td><td>" + MENAIC + "</td><td>" + MEPolicyNumber + "</td><td>" + MEPolicyHolder + "</td><td>" + MECoverageType + "</td><td>" + MECoverage + "</td><td>" + MEEffectiveDate + "</td><td>" + MEEndDate + "</td><td>" + MEGroupNumber + "</td></tr>";
                                }

                                table = table + "</tbody></table>";
                                document.getElementById('pnlResults5').innerHTML = '';
                                document.getElementById('pnlResults5').innerHTML = table;

                                if (MEPanelLet5.length > pageSize)
                                    setupPagination5(totalPages, 1);

                                $('.resultContainer5').toggleClass("active");
                                $('.csresult5').toggleClass("active");
                            }
                            else {
                                if (document.getElementById('paneltbl5') != null) {
                                    document.getElementById('paneltbl5').innerHTML = '';
                                }
                                METhirdPartyLiabilities = null;
                                MEPanelLet5 = null;
                            }
                        }
                        catch (err) {
                            console.log(err);
                        }

                        try {
                            var MEPatientLiabilities = result.PatientLiabilities
                            if (MEPatientLiabilities != null && MEPatientLiabilities != '' && MEPatientLiabilities.length > 0) {
                                MEPanelLet6 = MEPatientLiabilities;
                                var MEPatientLiabilitiesSliced = MEPanelLet6.slice(0, pageSize);
                                var totalPages = Math.ceil(MEPanelLet6.length / pageSize);
                                var table = "<table id='paneltbl6' class='table csTable'><thead><tr><th class='cursor' onclick='sortTable6(0)'>Financial Payer <span id='icon-0' class='sort-icon6'>&#8597;</span></th><th class='cursor' onclick='sortTable6(1)'>Monthly Amount <span id='icon-1' class='sort-icon6'>&#8597;</span></th><th>Type</th><th class='cursor' onclick='sortTable6(3)'>Effective Date <span id='icon-3' class='sort-icon6'>&#8597;</span></th><th class='cursor' onclick='sortTable6(4)'>End Date <span id='icon-4' class='sort-icon6'>&#8597;</span></th></tr></thead><tbody>";
                                for (var i = 0; i < MEPatientLiabilitiesSliced.length; i++) {

                                    var MEFinancialPayer = '';
                                    var MEMonthlyAmount = '';
                                    var METype = '';
                                    var MEEffectiveDate = '';
                                    var MEEndDate = '';

                                    if (MEPatientLiabilitiesSliced[i].FinancialPayer != null) {
                                        MEFinancialPayer = MEPatientLiabilitiesSliced[i].FinancialPayer;
                                    }
                                    if (MEPatientLiabilitiesSliced[i].MonthlyAmount != null) {
                                        MEMonthlyAmount = MEPatientLiabilitiesSliced[i].MonthlyAmount;
                                    }
                                    if (MEPatientLiabilitiesSliced[i].Type != null) {
                                        METype = MEPatientLiabilitiesSliced[i].Type;
                                    }
                                    if (MEPatientLiabilitiesSliced[i].EffectiveDate != null) {
                                        MEEffectiveDate = getFormattedDate(MEPatientLiabilitiesSliced[i].EffectiveDate.split('T')[0]);
                                    }
                                    if (MEPatientLiabilitiesSliced[i].EndDate != null) {
                                        MEEndDate = getFormattedDate(MEPatientLiabilitiesSliced[i].EndDate.split('T')[0]);
                                    }

                                    table = table + "<tr><td>" + MEFinancialPayer + "</td><td>" + MEMonthlyAmount + "</td><td>" + METype + "</td><td>" + MEEffectiveDate + "</td><td>" + MEEndDate + "</td></tr>";
                                }

                                table = table + "</tbody></table>";
                                document.getElementById('pnlResults6').innerHTML = '';
                                document.getElementById('pnlResults6').innerHTML = table;

                                if (MEPanelLet6.length > pageSize)
                                    setupPagination6(totalPages, 1);

                                $('.resultContainer6').toggleClass("active");
                                $('.csresult6').toggleClass("active");
                            }
                            else {
                                if (document.getElementById('paneltbl6') != null) {
                                    document.getElementById('paneltbl6').innerHTML = '';
                                }
                                MEPatientLiabilities = null;
                                MEPanelLet6 = null;
                            }
                        }
                        catch (err) {
                            console.log(err);
                        }

                        try {
                            var MELTCFPlacements = result.LTCFPlacements
                            if (MELTCFPlacements != null && MELTCFPlacements != '' && MELTCFPlacements.length > 0) {
                                MEPanelLet7 = MELTCFPlacements;
                                var MELTCFPlacementsSliced = MEPanelLet7.slice(0, pageSize);
                                var totalPages = Math.ceil(MEPanelLet7.length / pageSize);
                                var table = "<table id='paneltbl7' class='table csTable'><thead><tr><th>Facility Type</th><th>Date of Admission</th><th>Discharge Date</th><th>Effective Date of Medicaid Coverage</th><th>End Date of Medicaid Coverage</th></tr></thead><tbody>";
                                for (var i = 0; i < MELTCFPlacementsSliced.length; i++) {

                                    var MEFacilityType = '';
                                    var MEEffectiveDate = '';
                                    var MEEndDate = '';
                                    var MEEffectiveDateMedicaidCoverage = '';
                                    var MEEndDateMedicaidCoverage = '';

                                    if (MELTCFPlacementsSliced[i].FacilityType != null) {
                                        MEFacilityType = MELTCFPlacementsSliced[i].FacilityType;
                                    }
                                    if (MELTCFPlacementsSliced[i].EffectiveDate != null) {
                                        MEEffectiveDate = MELTCFPlacementsSliced[i].EffectiveDate.split('T')[0];
                                    }
                                    if (MELTCFPlacementsSliced[i].EndDate != null) {
                                        MEEndDate = MELTCFPlacementsSliced[i].EndDate.split('T')[0];
                                    }
                                    if (MELTCFPlacementsSliced[i].EffectiveDateMedicaidCoverage != null) {
                                        MEEffectiveDateMedicaidCoverage = getFormattedDate(MELTCFPlacementsSliced[i].EffectiveDateMedicaidCoverage.split('T')[0]);
                                    }
                                    if (MELTCFPlacementsSliced[i].EndDateMedicaidCoverage != null) {
                                        MEEndDateMedicaidCoverage = getFormattedDate(MELTCFPlacementsSliced[i].EndDateMedicaidCoverage.split('T')[0]);
                                    }

                                    table = table + "<tr><td>" + MEFacilityType + "</td><td>" + MEEffectiveDate + "</td><td>" + MEEndDate + "</td><td>" + MEEffectiveDateMedicaidCoverage + "</td><td>" + MEEndDateMedicaidCoverage + "</td></tr>";
                                }

                                table = table + "</tbody></table>";
                                document.getElementById('pnlResults7').innerHTML = '';
                                document.getElementById('pnlResults7').innerHTML = table;

                                if (MEPanelLet7.length > pageSize)
                                    setupPagination7(totalPages, 1);

                                $('.resultContainer7').toggleClass("active");
                                $('.csresult7').toggleClass("active");
                            }
                            else {
                                if (document.getElementById('paneltbl7') != null) {
                                    document.getElementById('paneltbl7').innerHTML = '';
                                }
                                MELTCFPlacements = null;
                                MEPanelLet7 = null;
                            }
                        }
                        catch (err) {
                            console.log(err);
                        }

                        try {
                            var MELockins = result.Lockins
                            if (MELockins != null && MELockins != '' && MELockins.length > 0) {
                                MEPanelLet8 = MELockins;
                                var MELockinsSliced = MEPanelLet8.slice(0, pageSize);
                                var totalPages = Math.ceil(MEPanelLet8.length / pageSize);
                                var table = "<table id='paneltbl8' class='table csTable'><thead><tr><th>Lock-In Plan</th><th>Lock-In Type</th><th>Effective Date</th><th>End Date</th><th>Provider NPI</th><th>Provider Name</th><th>Provider Phone</th></tr></thead><tbody>";
                                for (var i = 0; i < MELockinsSliced.length; i++) {

                                    var MELLockinPlan = '';
                                    var MELLockinType = '';
                                    var MELEffectiveDate = '';
                                    var MELEndDate = '';
                                    var MELProviderNPI = '';
                                    var MELProviderName = '';
                                    var MELProviderPhoneNumber = '';

                                    if (MELockinsSliced[i].LockinPlan != null) {
                                        MELLockinPlan = MELockinsSliced[i].LockinPlan;
                                    }
                                    if (MELockinsSliced[i].LockinType != null) {
                                        MELLockinType = MELockinsSliced[i].LockinType;
                                    }
                                    if (MELockinsSliced[i].EffectiveDate != null) {
                                        MELEffectiveDate = getFormattedDate(MELockinsSliced[i].EffectiveDate.split('T')[0]);
                                    }
                                    if (MELockinsSliced[i].EndDate != null) {
                                        MELEndDate = getFormattedDate(MELockinsSliced[i].EndDate.split('T')[0]);
                                    }
                                    if (MELockinsSliced[i].ProviderNPI != null) {
                                        MELProviderNPI = MELockinsSliced[i].ProviderNPI;
                                    }
                                    if (MELockinsSliced[i].ProviderName != null) {
                                        MELProviderName = MELockinsSliced[i].ProviderName;
                                    }
                                    if (MELockinsSliced[i].ProviderPhoneNumber != null) {
                                        MELProviderPhoneNumber = MELockinsSliced[i].ProviderPhoneNumber;
                                    }

                                    table = table + "<tr><td>" + MELLockinPlan + "</td><td>" + MELLockinType + "</td><td>" + MELEffectiveDate + "</td><td>" + MELEndDate + "</td><td>" + MELProviderNPI + "</td><td>" + MELProviderName + "</td><td>" + MELProviderPhoneNumber + "</td></tr>";
                                }

                                table = table + "</tbody></table>";
                                document.getElementById('pnlResults8').innerHTML = '';
                                document.getElementById('pnlResults8').innerHTML = table;

                                if (MEPanelLet8.length > pageSize)
                                    setupPagination8(totalPages, 1);

                                $('.resultContainer8').toggleClass("active");
                                $('.csresult8').toggleClass("active");
                            }
                            else {
                                if (document.getElementById('paneltbl8') != null) {
                                    document.getElementById('paneltbl8').innerHTML = '';
                                }
                                MELockins = null;
                                MEPanelLet8 = null;
                            }
                        }
                        catch (err) {
                            console.log(err);
                        }

                        try {
                            var MEMedicareCoverageDetails = result.MedicareCoverageDetails
                            if (MEMedicareCoverageDetails != null && MEMedicareCoverageDetails != '' && MEMedicareCoverageDetails.length > 0) {
                                MEPanelLet9 = MEMedicareCoverageDetails;
                                var MEMedicareCoverageDetailsSliced = MEPanelLet9.slice(0, pageSize);
                                var totalPages = Math.ceil(MEPanelLet9.length / pageSize);
                                var table = "<table id='paneltbl9' class='table csTable'><thead><tr><th class='cursor' onclick='sortTable9(0)'>Coverage <span id='icon-0' class='sort-icon9'>&#8597;</span></th><th class='cursor' onclick='sortTable9(1)'>Effective Date <span id='icon-1' class='sort-icon9'>&#8597;</span></th><th class='cursor' onclick='sortTable9(2)'>End Date <span id='icon-2' class='sort-icon9'>&#8597;</span></th><th class='cursor' onclick='sortTable9(3)'>Plan Name <span id='icon-3' class='sort-icon9'>&#8597;</span></th><th class='cursor' onclick='sortTable9(4)'>Plan ID <span id='icon-4' class='sort-icon9'>&#8597;</span></th><th>Medicare ID</th></tr></thead><tbody>";
                                for (var i = 0; i < MEMedicareCoverageDetailsSliced.length; i++) {
                                    var MEMCDCoverage = '';
                                    var MEMCDPlanId = '';
                                    var MEMCDPlanName = '';
                                    var MEMCDMedicareId = '';
                                    var MEMCDEffectiveDate = '';
                                    var MEMCDEndDate = '';

                                    if (MEMedicareCoverageDetailsSliced[i].Coverage != null) {
                                        MEMCDCoverage = MEMedicareCoverageDetailsSliced[i].Coverage;
                                    }
                                    if (MEMedicareCoverageDetailsSliced[i].PlanId != null) {
                                        MEMCDPlanId = MEMedicareCoverageDetailsSliced[i].PlanId;
                                    }
                                    else if (MEMedicareCoverageDetailsSliced[i].PlanId == null) {
                                        MEMedicareCoverageDetailsSliced[i].PlanId = '';
                                    }
                                    if (MEMedicareCoverageDetailsSliced[i].PlanName != null) {
                                        MEMCDPlanName = MEMedicareCoverageDetailsSliced[i].PlanName;
                                    }
                                    else if (MEMedicareCoverageDetailsSliced[i].PlanName == null) {
                                        MEMedicareCoverageDetailsSliced[i].PlanName = '';
                                    }
                                    if (MEMedicareCoverageDetailsSliced[i].MedicareId != null) {
                                        MEMCDMedicareId = MEMedicareCoverageDetailsSliced[i].MedicareId;
                                    }
                                    if (MEMedicareCoverageDetailsSliced[i].EffectiveDate != null) {
                                        MEMCDEffectiveDate = getFormattedDate(MEMedicareCoverageDetailsSliced[i].EffectiveDate.split('T')[0]);
                                    }
                                    if (MEMedicareCoverageDetailsSliced[i].EndDate != null) {
                                        MEMCDEndDate = getFormattedDate(MEMedicareCoverageDetailsSliced[i].EndDate.split('T')[0]);
                                    }

                                    table = table + "<tr><td>" + MEMCDCoverage + "</td><td>" + MEMCDEffectiveDate + "</td><td>" + MEMCDEndDate + "</td><td>" + MEMCDPlanName + "</td><td>" + MEMCDPlanId + "</td><td>" + MEMCDMedicareId + "</td></tr>";
                                }
                                //alert('MEMedicareCoverageDetails: ' + JSON.stringify(result.MedicareCoverageDetails));
                                table = table + "</tbody></table>";
                                document.getElementById('pnlResults9').innerHTML = '';
                                document.getElementById('pnlResults9').innerHTML = table;

                                if (MEPanelLet9.length > pageSize)
                                    setupPagination9(totalPages, 1);

                                $('.resultContainer9').toggleClass("active");
                                $('.csresult9').toggleClass("active");
                            }
                            else {
                                if (document.getElementById('paneltbl9') != null) {
                                    document.getElementById('paneltbl9').innerHTML = '';
                                }
                                MEMedicareCoverageDetails = null;
                                MEPanelLet9 = null;
                            }
                        }
                        catch (err) {
                            console.log(err);
                        }

                        try {
                            var MEServiceLimitations = result.ServiceLimitations
                            if (MEServiceLimitations != null && MEServiceLimitations != '' && MEServiceLimitations.length > 0) {
                                MEPanelLet11 = MEServiceLimitations;
                                var MEServiceLimitationsSliced = MEPanelLet11.slice(0, pageSize);
                                var totalPages = Math.ceil(MEPanelLet11.length / pageSize);
                                var table = "<table id='paneltbl11' class='table csTable'><thead><tr><th>Procedure Code</th><th>Description</th><th>Benefit Description</th><th>Total Limits</th><th>Used Limits</th><th>Remaining Limits</th><th>Time Frame</th><th>Date of Next Service</th></tr></thead><tbody>";
                                for (var i = 0; i < MEServiceLimitationsSliced.length; i++) {

                                    var MESLProcedureCode = '';
                                    var MESLServiceLimitDescription = '';
                                    var MESLBenefitDescription = '';
                                    var MESLTotalLimits = '';
                                    var MESLUsedLimits = '';
                                    var MESLRemainingLimits = '';
                                    var MESLTimeframe = '';
                                    var MESLDateOfNextService = '';

                                    if (MEServiceLimitationsSliced[i].ProcedureCode != null) {
                                        MESLProcedureCode = MEServiceLimitationsSliced[i].ProcedureCode;
                                    }
                                    if (MEServiceLimitationsSliced[i].ServiceLimitDescription != null) {
                                        MESLServiceLimitDescription = MEServiceLimitationsSliced[i].ServiceLimitDescription;
                                    }
                                    if (MEServiceLimitationsSliced[i].BenefitDescription != null) {
                                        MESLBenefitDescription = MEServiceLimitationsSliced[i].BenefitDescription;
                                    }
                                    if (MEServiceLimitationsSliced[i].TotalLimits != null) {
                                        MESLTotalLimits = MEServiceLimitationsSliced[i].TotalLimits;
                                    }
                                    if (MEServiceLimitationsSliced[i].UsedLimits != null) {
                                        MESLUsedLimits = MEServiceLimitationsSliced[i].UsedLimits;
                                    }
                                    if (MEServiceLimitationsSliced[i].RemainingLimits != null) {
                                        MESLRemainingLimits = MEServiceLimitationsSliced[i].RemainingLimits;
                                    }
                                    if (MEServiceLimitationsSliced[i].Timeframe != null) {
                                        MESLTimeframe = MEServiceLimitationsSliced[i].Timeframe;
                                    }
                                    if (MEServiceLimitationsSliced[i].DateOfNextService != null) {
                                        MESLDateOfNextService = getFormattedDate(MEServiceLimitationsSliced[i].DateOfNextService.split('T')[0]);
                                    }

                                    table = table + "<tr><td>" + MESLProcedureCode + "</td><td>" + MESLServiceLimitDescription + "</td><td>" + MESLBenefitDescription + "</td><td>" + MESLTotalLimits + "</td><td>" + MESLUsedLimits + "</td><td>" + MESLRemainingLimits + "</td><td>" + MESLTimeframe + "</td><td>" + MESLDateOfNextService + "</td></tr>";
                                }

                                table = table + "</tbody></table>";
                                document.getElementById('pnlResults11').innerHTML = '';
                                document.getElementById('pnlResults11').innerHTML = table;

                                if (MEPanelLet11.length > pageSize)
                                    setupPagination11(totalPages, 1);

                                $('.resultContainer11').toggleClass("active");
                                $('.csresult11').toggleClass("active");
                            }
                            else {
                                if (document.getElementById('paneltbl11') != null) {
                                    document.getElementById('paneltbl11').innerHTML = '';
                                }
                                MEServiceLimitations = null;
                                MEPanelLet11 = null;
                            }
                        }
                        catch (err) {
                            console.log(err);
                        }

                        try {
                            var MERestrictedCoverages = result.RestrictedCoverages
                            if (MERestrictedCoverages != null && MERestrictedCoverages != '' && MERestrictedCoverages.length > 0) {
                                MEPanelLet12 = MERestrictedCoverages;
                                var MERestrictedCoveragesSliced = MEPanelLet12.slice(0, pageSize);
                                var totalPages = Math.ceil(MEPanelLet12.length / pageSize);
                                var table = "<table id='paneltbl12' class='table csTable'><thead><tr><th>Effective Date</th><th>End Date</th></tr></thead><tbody>";
                                for (var i = 0; i < MERestrictedCoveragesSliced.length; i++) {
                                    var MERCEffectiveDate = '';
                                    var MERCEndDate = '';

                                    if (MERestrictedCoveragesSliced[i].EffectiveDate != null) {
                                        MERCEffectiveDate = getFormattedDate(MERestrictedCoveragesSliced[i].EffectiveDate.split('T')[0]);
                                    }
                                    if (MERestrictedCoveragesSliced[i].EndDate != null) {
                                        MERCEndDate = getFormattedDate(MERestrictedCoveragesSliced[i].EndDate.split('T')[0]);
                                    }

                                    table = table + "<tr><td>" + MERCEffectiveDate + "</td><td>" + MERCEndDate + "</td></tr>";
                                }

                                table = table + "</tbody></table>";
                                document.getElementById('pnlResults12').innerHTML = '';
                                document.getElementById('pnlResults12').innerHTML = table;

                                if (MEPanelLet12.length > pageSize)
                                    setupPagination12(totalPages, 1);

                                $('.resultContainer12').toggleClass("active");
                                $('.csresult12').toggleClass("active");
                            }
                            else {
                                if (document.getElementById('paneltbl12') != null) {
                                    document.getElementById('paneltbl12').innerHTML = '';
                                }
                                MERestrictedCoverages = null;
                                MEPanelLet12 = null;
                            }
                        }
                        catch (err) {
                            console.log(err);
                        }

                        try {
                            var MEUnder19FamilyMembers = result.Under19FamilyMembers
                            if (MEUnder19FamilyMembers != null && MEUnder19FamilyMembers != '' && MEUnder19FamilyMembers.length > 0) {
                                MEPanelLet13 = MEUnder19FamilyMembers;
                                var MEUnder19FamilyMembersSliced = MEPanelLet13.slice(0, pageSize);
                                var totalPages = Math.ceil(MEPanelLet13.length / pageSize);
                                var table = "<table id='paneltbl13' class='table csTable'><thead><tr><th>Medicaid Billing Number</th><th>First Name</th><th>MI</th><th>Last Name</th><th>Gender</th><th>Date of Birth</th></tr></thead><tbody>";
                                for (var i = 0; i < MEUnder19FamilyMembersSliced.length; i++) {
                                    var MEMedicaidId = '';
                                    var MEUU9DateOfBirth = '';
                                    var MEFirstName = '';
                                    var MEMiddleInitial = '';
                                    var MELastName = '';
                                    var MEGender = '';

                                    if (MEUnder19FamilyMembersSliced[i].MedicaidId != null) {
                                        MEMedicaidId = MEUnder19FamilyMembersSliced[i].MedicaidId;
                                    }
                                    if (MEUnder19FamilyMembersSliced[i].DateOfBirth != null) {
                                        MEUU9DateOfBirth = getFormattedDate(MEUnder19FamilyMembersSliced[i].DateOfBirth.split('T')[0]);
                                    }
                                    if (MEUnder19FamilyMembersSliced[i].FirstName != null) {
                                        MEFirstName = MEUnder19FamilyMembersSliced[i].FirstName;
                                    }
                                    if (MEUnder19FamilyMembersSliced[i].MiddleInitial != null) {
                                        MEMiddleInitial = MEUnder19FamilyMembersSliced[i].MiddleInitial;
                                    }
                                    if (MEUnder19FamilyMembersSliced[i].LastName != null) {
                                        MELastName = MEUnder19FamilyMembersSliced[i].LastName;
                                    }
                                    if (MEUnder19FamilyMembersSliced[i].Gender != null) {
                                        MEGender = MEUnder19FamilyMembersSliced[i].Gender;
                                    }

                                    table = table + "<tr><td>" + MEMedicaidId + "</td><td>" + MEFirstName + "</td><td>" + MEMiddleInitial + "</td><td>" + MELastName + "</td><td>" + MEGender + "</td><td>" + MEUU9DateOfBirth + "</td></tr>";
                                }

                                table = table + "</tbody></table>";
                                document.getElementById('pnlResults13').innerHTML = '';
                                document.getElementById('pnlResults13').innerHTML = table;

                                if (MEPanelLet13.length > pageSize)
                                    setupPagination13(totalPages, 1);

                                $('.resultContainer13').toggleClass("active");
                                $('.csresult13').toggleClass("active");
                            }
                            else {
                                if (document.getElementById('paneltbl13') != null) {
                                    document.getElementById('paneltbl13').innerHTML = '';
                                }
                                MEUnder19FamilyMembers = null;
                                MEPanelLet13 = null;
                            }
                        }
                        catch (err) {
                            console.log(err);
                        }

                        document.getElementById('btnPrint').style.display = 'inline';
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                document.getElementById("btnSearch").disabled = false;
                document.getElementById('btnPrint').style.display = 'none';
                var status = jqXHR.status;
                document.getElementById('pnlMEResults').innerHTML = '';
                document.getElementById('pnlMEResults').innerHTML = '<span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">Error : ' + status + ' No data returned</span>';
                console.log(JSON.stringify(jqXHR));
            }
        });
    }
    function resultToggle1() {
        $('.resultContainer1').toggleClass("active");
        $('.csresult1').toggleClass("active");
    }
    function resultToggle2() {
        $('.resultContainer2').toggleClass("active");
        $('.csresult2').toggleClass("active");
    }
    function resultToggle3() {
        $('.resultContainer3').toggleClass("active");
        $('.csresult3').toggleClass("active");
    }
    function resultToggle4() {
        $('.resultContainer4').toggleClass("active");
        $('.csresult4').toggleClass("active");
    }
    function resultToggle5() {
        $('.resultContainer5').toggleClass("active");
        $('.csresult5').toggleClass("active");
    }
    function resultToggle6() {
        $('.resultContainer6').toggleClass("active");
        $('.csresult6').toggleClass("active");
    }
    function resultToggle7() {
        $('.resultContainer7').toggleClass("active");
        $('.csresult7').toggleClass("active");
    }
    function resultToggle8() {
        $('.resultContainer8').toggleClass("active");
        $('.csresult8').toggleClass("active");
    }
    function resultToggle9() {
        $('.resultContainer9').toggleClass("active");
        $('.csresult9').toggleClass("active");
    }

    function resultToggle11() {
        $('.resultContainer11').toggleClass("active");
        $('.csresult11').toggleClass("active");
    }
    function resultToggle12() {
        $('.resultContainer12').toggleClass("active");
        $('.csresult12').toggleClass("active");
    }
    function resultToggle13() {
        $('.resultContainer13').toggleClass("active");
        $('.csresult13').toggleClass("active");
    }

    function PreviousClick3(totalPages, curPage) {

        if (curPage > 1) {
            currentPage3--;
            panel3Refresh(currentPage3);
        }
    }

    function NextClick3(totPages, currPage) {

        if (currPage < totPages) {
            currentPage3++;
            panel3Refresh(currentPage3);
        }
    }

    function PreviousClick4(totalPages, curPage) {

        if (curPage > 1) {
            currentPage4--;
            panel4Refresh(currentPage4);
        }
    }

    function NextClick4(totPages, currPage) {

        if (currPage < totPages) {
            currentPage4++;
            panel4Refresh(currentPage4);
        }
    }
    function PreviousClick5(totalPages, curPage) {

        if (curPage > 1) {
            currentPage5--;
            panel5Refresh(currentPage5);
        }
    }

    function NextClick5(totPages, currPage) {

        if (currPage < totPages) {
            currentPage5++;
            panel5Refresh(currentPage5);
        }
    }
    function PreviousClick6(totalPages, curPage) {

        if (curPage > 1) {
            currentPage6--;
            panel6Refresh(currentPage6);
        }
    }

    function NextClick6(totPages, currPage) {

        if (currPage < totPages) {
            currentPage6++;
            panel6Refresh(currentPage6);
        }
    }
    function PreviousClick7(totalPages, curPage) {

        if (curPage > 1) {
            currentPage7--;
            panel7Refresh(currentPage7);
        }
    }

    function NextClick7(totPages, currPage) {

        if (currPage < totPages) {
            currentPage7++;
            panel7Refresh(currentPage7);
        }
    }
    function PreviousClick8(totalPages, curPage) {

        if (curPage > 1) {
            currentPage8--;
            panel8Refresh(currentPage8);
        }
    }

    function NextClick8(totPages, currPage) {

        if (currPage < totPages) {
            currentPage8++;
            panel8Refresh(currentPage8);
        }
    }
    function PreviousClick9(totalPages, curPage) {

        if (curPage > 1) {
            currentPage9--;
            panel9Refresh(currentPage9);
        }
    }

    function NextClick9(totPages, currPage) {

        if (currPage < totPages) {
            currentPage9++;
            panel9Refresh(currentPage9);
        }
    }
    function PreviousClick10(totalPages, curPage) {

        if (curPage > 1) {
            currentPage10--;
            panel10Refresh(currentPage10);
        }
    }

    function NextClick10(totPages, currPage) {

        if (currPage < totPages) {
            currentPage10++;
            panel10Refresh(currentPage10);
        }
    }
    function PreviousClick11(totalPages, curPage) {

        if (curPage > 1) {
            currentPage11--;
            panel11Refresh(currentPage11);
        }
    }

    function NextClick11(totPages, currPage) {

        if (currPage < totPages) {
            currentPage11++;
            panel11Refresh(currentPage11);
        }
    }
    function PreviousClick12(totalPages, curPage) {

        if (curPage > 1) {
            currentPage12--;
            panel12Refresh(currentPage12);
        }
    }

    function NextClick12(totPages, currPage) {

        if (currPage < totPages) {
            currentPage12++;
            panel12Refresh(currentPage12);
        }
    }
    function PreviousClick13(totalPages, curPage) {

        if (curPage > 1) {
            currentPage13--;
            panel13Refresh(currentPage13);
        }
    }

    function NextClick13(totPages, currPage) {

        if (currPage < totPages) {
            currentPage13++;
            panel13Refresh(currentPage13);
        }
    }

    function btnPaging3_Click(pagenumber) {

        const buttons = document.querySelectorAll(".mybtnpage3");
        currentPage3 = pagenumber;
        panel3Refresh(pagenumber);
        buttons.forEach(button => {
            button.style.fontWeight = 'normal';
        });
        var pageId = 'btnPage3_' + pagenumber;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'
    }

    function btnPaging4_Click(pagenumber) {

        const buttons = document.querySelectorAll(".mybtnpage4");
        currentPage4 = pagenumber;
        panel4Refresh(pagenumber);
        buttons.forEach(button => {
            button.style.fontWeight = 'normal';
        });
        var pageId = 'btnPage4_' + pagenumber;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'
    }

    function btnPaging5_Click(pagenumber) {

        const buttons = document.querySelectorAll(".mybtnpage5");
        currentPage5 = pagenumber;
        panel5Refresh(pagenumber);
        buttons.forEach(button => {
            button.style.fontWeight = 'normal';
        });
        var pageId = 'btnPage5_' + pagenumber;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'
    }

    function btnPaging6_Click(pagenumber) {

        const buttons = document.querySelectorAll(".mybtnpage6");
        currentPage6 = pagenumber;
        panel6Refresh(pagenumber);
        buttons.forEach(button => {
            button.style.fontWeight = 'normal';
        });
        var pageId = 'btnPage6_' + pagenumber;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'
    }

    function btnPaging7_Click(pagenumber) {

        const buttons = document.querySelectorAll(".mybtnpage7");
        currentPage7 = pagenumber;
        panel7Refresh(pagenumber);
        buttons.forEach(button => {
            button.style.fontWeight = 'normal';
        });
        var pageId = 'btnPage7_' + pagenumber;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'
    }

    function btnPaging8_Click(pagenumber) {

        const buttons = document.querySelectorAll(".mybtnpage8");
        currentPage8 = pagenumber;
        panel8Refresh(pagenumber);
        buttons.forEach(button => {
            button.style.fontWeight = 'normal';
        });
        var pageId = 'btnPage8_' + pagenumber;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'
    }

    function btnPaging9_Click(pagenumber) {

        const buttons = document.querySelectorAll(".mybtnpage9");
        currentPage9 = pagenumber;
        panel9Refresh(pagenumber);
        buttons.forEach(button => {
            button.style.fontWeight = 'normal';
        });
        var pageId = 'btnPage9_' + pagenumber;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'
    }

    function btnPaging10_Click(pagenumber) {

        const buttons = document.querySelectorAll(".mybtnpage10");
        currentPage10 = pagenumber;
        panel10Refresh(pagenumber);
        buttons.forEach(button => {
            button.style.fontWeight = 'normal';
        });
        var pageId = 'btnPage10_' + pagenumber;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'
    }

    function btnPaging11_Click(pagenumber) {

        const buttons = document.querySelectorAll(".mybtnpage11");
        currentPage11 = pagenumber;
        panel11Refresh(pagenumber);
        buttons.forEach(button => {
            button.style.fontWeight = 'normal';
        });
        var pageId = 'btnPage11_' + pagenumber;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'
    }

    function btnPaging12_Click(pagenumber) {

        const buttons = document.querySelectorAll(".mybtnpage12");
        currentPage12 = pagenumber;
        panel12Refresh(pagenumber);
        buttons.forEach(button => {
            button.style.fontWeight = 'normal';
        });
        var pageId = 'btnPage12_' + pagenumber;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'
    }

    function btnPaging13_Click(pagenumber) {

        const buttons = document.querySelectorAll(".mybtnpage13");
        currentPage13 = pagenumber;
        panel13Refresh(pagenumber);
        buttons.forEach(button => {
            button.style.fontWeight = 'normal';
        });
        var pageId = 'btnPage13_' + pagenumber;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'
    }

    function panel3Refresh(pagenumber) {
        var startIndex = (pagenumber - 1) * pageSize;
        var endIndex = startIndex + pageSize;
        var MEBenifitAssignmentPlansSliced = MEPanelLet3.slice(startIndex, endIndex);
        var totalPages = Math.ceil(MEPanelLet3.length / pageSize);
        var table = "<table id='paneltbl3' class='table csTable'><thead><tr><th class='cursor' onclick='sortTable3(0)'>Benefit/Assignment Plan <span id='icon-0' class='sort-icon3'>&#8597;</span></th><th class='cursor' onclick='sortTable3(1)'>Effective Date <span id='icon-1' class='sort-icon3'>&#8597;</span></th><th class='cursor' onclick='sortTable3(2)'>End Date <span id='icon-2' class='sort-icon3'>&#8597;</span></th></tr></thead><tbody>";
        for (var i = 0; i < MEBenifitAssignmentPlansSliced.length; i++) {

            var BAAssignmentPlan = '';
            var BAEffectiveDate = '';
            var BAEndDate = '';

            if (MEBenifitAssignmentPlansSliced[i].AssignmentPlan != null) {
                BAAssignmentPlan = MEBenifitAssignmentPlansSliced[i].AssignmentPlan;
            }
            if (MEBenifitAssignmentPlansSliced[i].EffectiveDate != null) {
                BAEffectiveDate = getFormattedDate(MEBenifitAssignmentPlansSliced[i].EffectiveDate.split('T')[0]);
            }
            if (MEBenifitAssignmentPlansSliced[i].EndDate != null) {
                BAEndDate = getFormattedDate(MEBenifitAssignmentPlansSliced[i].EndDate.split('T')[0]);
            }

            table = table + "<tr><td>" + BAAssignmentPlan + "</td><td>" + BAEffectiveDate + "</td><td>" + BAEndDate + "</td></tr>";
        }

        table = table + "</tbody></table>";
        document.getElementById('pnlResults3').innerHTML = '';
        document.getElementById('pnlResults3').innerHTML = table;

        if (MEPanelLet3.length > pageSize)
            setupPagination3(totalPages, pagenumber);
    }

    function panel4Refresh(pagenumber) {
        var startIndex = (pagenumber - 1) * pageSize;
        var endIndex = startIndex + pageSize;
        var MEListOfManagedCarePlanSliced = MEPanelLet4.slice(startIndex, endIndex);
        var totalPages = Math.ceil(MEPanelLet4.length / pageSize);
        var table = "<table id='paneltbl4' class='table csTable'><thead><tr><th class='cursor' onclick='sortTable4(0)'>Plan Name <span id='icon-0' class='sort-icon4'>&#8597;</span></th><th class='cursor' onclick='sortTable4(1)'>Payer ID <span id='icon-1' class='sort-icon4'>&#8597;</span></th><th>Plan Description</th><th class='cursor' onclick='sortTable4(3)'>Effective Date <span id='icon-3' class='sort-icon4'>&#8597;</span></th><th class='cursor' onclick='sortTable4(4)'>End Date <span id='icon-4' class='sort-icon4'>&#8597;</span></th><th>Managed Care Benefits</th></tr></thead><tbody>";
        for (var i = 0; i < MEListOfManagedCarePlanSliced.length; i++) {

            var MEManagedCarePlanId = '';
            var MEPlanName = '';
            var MEPlanDescription = '';
            var MEEffectiveDate = '';
            var MEEndDate = '';
            var MEManagedCareBenefits = '';

            if (MEListOfManagedCarePlanSliced[i].PlanId != null) {
                MEManagedCarePlanId = MEListOfManagedCarePlanSliced[i].PlanId;
            }
            if (MEListOfManagedCarePlanSliced[i].PlanName != null) {
                MEPlanName = MEListOfManagedCarePlanSliced[i].PlanName;
            }
            if (MEListOfManagedCarePlanSliced[i].PlanDescription != null) {
                MEPlanDescription = MEListOfManagedCarePlanSliced[i].PlanDescription;
            }
            if (MEListOfManagedCarePlanSliced[i].EffectiveDate != null) {
                MEEffectiveDate = getFormattedDate(MEListOfManagedCarePlanSliced[i].EffectiveDate.split('T')[0]);
            }
            if (MEListOfManagedCarePlanSliced[i].EndDate != null) {
                MEEndDate = getFormattedDate(MEListOfManagedCarePlanSliced[i].EndDate.split('T')[0]);
            }
            if (MEListOfManagedCarePlanSliced[i].ManagedCareBenefits != null) {
                MEManagedCareBenefits = MEListOfManagedCarePlanSliced[i].ManagedCareBenefits;
            }

            table = table + "<tr><td>" + MEPlanName + "</td><td>" + MEManagedCarePlanId + "</td><td>" + MEPlanDescription + "</td><td>" + MEEffectiveDate + "</td><td>" + MEEndDate + "</td><td>" + MEManagedCareBenefits + "</td></tr>";
        }

        table = table + "</tbody></table>";
        document.getElementById('pnlResults4').innerHTML = '';
        document.getElementById('pnlResults4').innerHTML = table;

        if (MEPanelLet4.length > pageSize)
            setupPagination4(totalPages, pagenumber);
    }

    function panel5Refresh(pagenumber) {
        var startIndex = (pagenumber - 1) * pageSize;
        var endIndex = startIndex + pageSize;
        var METhirdPartyLiabilitiesSliced = MEPanelLet5.slice(startIndex, endIndex);
        var totalPages = Math.ceil(MEPanelLet5.length / pageSize);
        var table = "<table id='paneltbl5' class='table csTable'><thead><tr><th>Carrier Name</th><th>Carrier Number</th><th>NAIC</th><th>Policy Number</th><th>Policy Holder</th><th>Coverage Type</th><th>Coverage</th><th>Effective Date</th><th>End Date</th><th>Group Number</th></tr></thead><tbody>";
        for (var i = 0; i < METhirdPartyLiabilitiesSliced.length; i++) {

            var MECarrierName = '';
            var MECarrierNumber = '';
            var MENAIC = '';
            var MEPolicyNumber = '';
            var MEPolicyHolder = '';
            var MECoverageType = '';
            var MECoverage = '';
            var MEEffectiveDate = '';
            var MEEndDate = '';
            var MEGroupNumber = '';

            if (METhirdPartyLiabilitiesSliced[i].CarrierName != null) {
                MECarrierName = METhirdPartyLiabilitiesSliced[i].CarrierName;
            }
            if (METhirdPartyLiabilitiesSliced[i].CarrierNumber != null) {
                MECarrierNumber = METhirdPartyLiabilitiesSliced[i].CarrierNumber;
            }
            if (METhirdPartyLiabilitiesSliced[i].NAIC != null) {
                MENAIC = METhirdPartyLiabilitiesSliced[i].NAIC;
            }
            if (METhirdPartyLiabilitiesSliced[i].PolicyNumber != null) {
                MEPolicyNumber = METhirdPartyLiabilitiesSliced[i].PolicyNumber;
            }
            if (METhirdPartyLiabilitiesSliced[i].PolicyHolder != null) {
                MEPolicyHolder = METhirdPartyLiabilitiesSliced[i].PolicyHolder;
            }
            if (METhirdPartyLiabilitiesSliced[i].CoverageType != null) {
                MECoverageType = METhirdPartyLiabilitiesSliced[i].CoverageType;
            }
            if (METhirdPartyLiabilitiesSliced[i].Coverage != null) {
                MECoverage = METhirdPartyLiabilitiesSliced[i].Coverage;
            }
            if (METhirdPartyLiabilitiesSliced[i].EffectiveDate != null) {
                MEEffectiveDate = getFormattedDate(METhirdPartyLiabilitiesSliced[i].EffectiveDate.split('T')[0]);
            }
            if (METhirdPartyLiabilitiesSliced[i].EndDate != null) {
                MEEndDate = getFormattedDate(METhirdPartyLiabilitiesSliced[i].EndDate.split('T')[0]);
            }
            if (METhirdPartyLiabilitiesSliced[i].GroupNumber != null) {
                MEGroupNumber = METhirdPartyLiabilitiesSliced[i].GroupNumber;
            }

            table = table + "<tr><td>" + MECarrierName + "</td><td>" + MECarrierNumber + "</td><td>" + MENAIC + "</td><td>" + MEPolicyNumber + "</td><td>" + MEPolicyHolder + "</td><td>" + MECoverageType + "</td><td>" + MECoverage + "</td><td>" + MEEffectiveDate + "</td><td>" + MEEndDate + "</td><td>" + MEGroupNumber + "</td></tr>";
        }

        table = table + "</tbody></table>";
        document.getElementById('pnlResults5').innerHTML = '';
        document.getElementById('pnlResults5').innerHTML = table;

        if (MEPanelLet5.length > pageSize)
            setupPagination5(totalPages, pagenumber);
    }

    function panel6Refresh(pagenumber) {
        var startIndex = (pagenumber - 1) * pageSize;
        var endIndex = startIndex + pageSize;
        var MEPatientLiabilitiesSliced = MEPanelLet6.slice(startIndex, endIndex);
        var totalPages = Math.ceil(MEPanelLet6.length / pageSize);
        var table = "<table id='paneltbl6' class='table csTable'><thead><tr><th class='cursor' onclick='sortTable6(0)'>Financial Payer <span id='icon-0' class='sort-icon6'>&#8597;</span></th><th class='cursor' onclick='sortTable6(1)'>Monthly Amount <span id='icon-1' class='sort-icon6'>&#8597;</span></th><th>Type</th><th class='cursor' onclick='sortTable6(3)'>Effective Date <span id='icon-3' class='sort-icon6'>&#8597;</span></th><th class='cursor' onclick='sortTable6(4)'>End Date <span id='icon-4' class='sort-icon6'>&#8597;</span></th></tr></thead><tbody>";
        for (var i = 0; i < MEPatientLiabilitiesSliced.length; i++) {

            var MEFinancialPayer = '';
            var MEMonthlyAmount = '';
            var METype = '';
            var MEEffectiveDate = '';
            var MEEndDate = '';

            if (MEPatientLiabilitiesSliced[i].FinancialPayer != null) {
                MEFinancialPayer = MEPatientLiabilitiesSliced[i].FinancialPayer;
            }
            if (MEPatientLiabilitiesSliced[i].MonthlyAmount != null) {
                MEMonthlyAmount = MEPatientLiabilitiesSliced[i].MonthlyAmount;
            }
            if (MEPatientLiabilitiesSliced[i].Type != null) {
                METype = MEPatientLiabilitiesSliced[i].Type;
            }
            if (MEPatientLiabilitiesSliced[i].EffectiveDate != null) {
                MEEffectiveDate = getFormattedDate(MEPatientLiabilitiesSliced[i].EffectiveDate.split('T')[0]);
            }
            if (MEPatientLiabilitiesSliced[i].EndDate != null) {
                MEEndDate = getFormattedDate(MEPatientLiabilitiesSliced[i].EndDate.split('T')[0]);
            }

            table = table + "<tr><td>" + MEFinancialPayer + "</td><td>" + MEMonthlyAmount + "</td><td>" + METype + "</td><td>" + MEEffectiveDate + "</td><td>" + MEEndDate + "</td></tr>";
        }

        table = table + "</tbody></table>";
        document.getElementById('pnlResults6').innerHTML = '';
        document.getElementById('pnlResults6').innerHTML = table;

        if (MEPanelLet6.length > pageSize)
            setupPagination6(totalPages, pagenumber);
    }

    function panel7Refresh(pagenumber) {
        var startIndex = (pagenumber - 1) * pageSize;
        var endIndex = startIndex + pageSize;
        var MELTCFPlacementsSliced = MEPanelLet7.slice(startIndex, endIndex);
        var totalPages = Math.ceil(MEPanelLet7.length / pageSize);
        var table = "<table id='paneltbl7' class='table csTable'><thead><tr><th>Facility Type</th><th>Date of Admission</th><th>Discharge Date</th><th>Effective Date of Medicaid Coverage</th><th>End Date of Medicaid Coverage</th></tr></thead><tbody>";
        for (var i = 0; i < MELTCFPlacementsSliced.length; i++) {

            var MEFacilityType = '';
            var MEEffectiveDate = '';
            var MEEndDate = '';
            var MEEffectiveDateMedicaidCoverage = '';
            var MEEndDateMedicaidCoverage = '';

            if (MELTCFPlacementsSliced[i].FacilityType != null) {
                MEFacilityType = MELTCFPlacementsSliced[i].FacilityType;
            }
            if (MELTCFPlacementsSliced[i].EffectiveDate != null) {
                MEEffectiveDate = getFormattedDate(MELTCFPlacementsSliced[i].EffectiveDate.split('T')[0]);
            }
            if (MELTCFPlacementsSliced[i].EndDate != null) {
                MEEndDate = getFormattedDate(MELTCFPlacementsSliced[i].EndDate.split('T')[0]);
            }
            if (MELTCFPlacementsSliced[i].EffectiveDateMedicaidCoverage != null) {
                MEEffectiveDateMedicaidCoverage = getFormattedDate(MELTCFPlacementsSliced[i].EffectiveDateMedicaidCoverage.split('T')[0]);
            }
            if (MELTCFPlacementsSliced[i].EndDateMedicaidCoverage != null) {
                MEEndDateMedicaidCoverage = getFormattedDate(MELTCFPlacementsSliced[i].EndDateMedicaidCoverage.split('T')[0]);
            }

            table = table + "<tr><td>" + MEFacilityType + "</td><td>" + MEEffectiveDate + "</td><td>" + MEEndDate + "</td><td>" + MEEffectiveDateMedicaidCoverage + "</td><td>" + MEEndDateMedicaidCoverage + "</td></tr>";
        }

        table = table + "</tbody></table>";
        document.getElementById('pnlResults7').innerHTML = '';
        document.getElementById('pnlResults7').innerHTML = table;
        if (MEPanelLet7.length > pageSize)
            setupPagination7(totalPages, pagenumber);
    }

    function panel8Refresh(pagenumber) {
        var startIndex = (pagenumber - 1) * pageSize;
        var endIndex = startIndex + pageSize;
        var MELockinsSliced = MEPanelLet8.slice(startIndex, endIndex);
        var totalPages = Math.ceil(MEPanelLet8.length / pageSize);
        var table = "<table id='paneltbl8' class='table csTable'><thead><tr><th>Lock-In Plan</th><th>Lock-In Type</th><th>Effective Date</th><th>End Date</th><th>Provider NPI</th><th>Provider Name</th><th>Provider Phone</th></tr></thead><tbody>";
        for (var i = 0; i < MELockinsSliced.length; i++) {

            var MELLockinPlan = '';
            var MELLockinType = '';
            var MELEffectiveDate = '';
            var MELEndDate = '';
            var MELProviderNPI = '';
            var MELProviderName = '';
            var MELProviderPhoneNumber = '';

            if (MELockinsSliced[i].LockinPlan != null) {
                MELLockinPlan = MELockinsSliced[i].LockinPlan;
            }
            if (MELockinsSliced[i].LockinType != null) {
                MELLockinType = MELockinsSliced[i].LockinType;
            }
            if (MELockinsSliced[i].EffectiveDate != null) {
                MELEffectiveDate = getFormattedDate(MELockinsSliced[i].EffectiveDate.split('T')[0]);
            }
            if (MELockinsSliced[i].EndDate != null) {
                MELEndDate = getFormattedDate(MELockinsSliced[i].EndDate.split('T')[0]);
            }
            if (MELockinsSliced[i].ProviderNPI != null) {
                MELProviderNPI = MELockinsSliced[i].ProviderNPI;
            }
            if (MELockinsSliced[i].ProviderName != null) {
                MELProviderName = MELockinsSliced[i].ProviderName;
            }
            if (MELockinsSliced[i].ProviderPhoneNumber != null) {
                MELProviderPhoneNumber = MELockinsSliced[i].ProviderPhoneNumber;
            }

            table = table + "<tr><td>" + MELLockinPlan + "</td><td>" + MELLockinType + "</td><td>" + MELEffectiveDate + "</td><td>" + MELEndDate + "</td><td>" + MELProviderNPI + "</td><td>" + MELProviderName + "</td><td>" + MELProviderPhoneNumber + "</td></tr>";
        }

        table = table + "</tbody></table>";
        document.getElementById('pnlResults8').innerHTML = '';
        document.getElementById('pnlResults8').innerHTML = table;

        if (MEPanelLet8.length > pageSize)
            setupPagination8(totalPages, pagenumber);
    }

    function panel9Refresh(pagenumber) {
        var startIndex = (pagenumber - 1) * pageSize;
        var endIndex = startIndex + pageSize;
        var MEMedicareCoverageDetailsSliced = MEPanelLet9.slice(startIndex, endIndex); 
        var totalPages = Math.ceil(MEPanelLet9.length / pageSize);
        var table = "<table id='paneltbl9' class='table csTable'><thead><tr><th class='cursor' onclick='sortTable9(0)'>Coverage <span id='icon-0' class='sort-icon9'>&#8597;</span></th><th class='cursor' onclick='sortTable9(1)'>Effective Date <span id='icon-1' class='sort-icon9'>&#8597;</span></th><th class='cursor' onclick='sortTable9(2)'>End Date <span id='icon-2' class='sort-icon9'>&#8597;</span></th><th class='cursor' onclick='sortTable9(3)'>Plan Name <span id='icon-3' class='sort-icon9'>&#8597;</span></th><th class='cursor' onclick='sortTable9(4)'>Plan ID <span id='icon-4' class='sort-icon9'>&#8597;</span></th><th>Medicare ID</th></tr></thead><tbody>";
        for (var i = 0; i < MEMedicareCoverageDetailsSliced.length; i++) {
            var MEMCDCoverage = '';
            var MEMCDPlanId = '';
            var MEMCDPlanName = '';
            var MEMCDMedicareId = '';
            var MEMCDEffectiveDate = '';
            var MEMCDEndDate = '';

            if (MEMedicareCoverageDetailsSliced[i].Coverage != null) {
                MEMCDCoverage = MEMedicareCoverageDetailsSliced[i].Coverage;
            }
            if (MEMedicareCoverageDetailsSliced[i].PlanId != null) {
                MEMCDPlanId = MEMedicareCoverageDetailsSliced[i].PlanId;
            }
            if (MEMedicareCoverageDetailsSliced[i].PlanName != null) {
                MEMCDPlanName = MEMedicareCoverageDetailsSliced[i].PlanName;
            }
            if (MEMedicareCoverageDetailsSliced[i].MedicareId != null) {
                MEMCDMedicareId = MEMedicareCoverageDetailsSliced[i].MedicareId;
            }
            if (MEMedicareCoverageDetailsSliced[i].EffectiveDate != null) {
                MEMCDEffectiveDate = getFormattedDate(MEMedicareCoverageDetailsSliced[i].EffectiveDate.split('T')[0]);
            }
            if (MEMedicareCoverageDetailsSliced[i].EndDate != null) {
                MEMCDEndDate = getFormattedDate(MEMedicareCoverageDetailsSliced[i].EndDate.split('T')[0]);
            }

            table = table + "<tr><td>" + MEMCDCoverage + "</td><td>" + MEMCDEffectiveDate + "</td><td>" + MEMCDEndDate + "</td><td>" + MEMCDPlanName + "</td><td>" + MEMCDPlanId + "</td><td>" + MEMCDMedicareId + "</td></tr>";
        }

        table = table + "</tbody></table>";
        document.getElementById('pnlResults9').innerHTML = '';
        document.getElementById('pnlResults9').innerHTML = table;

        if (MEPanelLet9.length > pageSize)
            setupPagination9(totalPages, pagenumber);
    }

    function panel11Refresh(pagenumber) {
        var startIndex = (pagenumber - 1) * pageSize;
        var endIndex = startIndex + pageSize;
        var MEServiceLimitationsSliced = MEPanelLet11.slice(startIndex, endIndex);
        var totalPages = Math.ceil(MEPanelLet11.length / pageSize);
        var table = "<table id='paneltbl11' class='table csTable'><thead><tr><th>Procedure Code</th><th>Description</th><th>Benefit Description</th><th>Total Limits</th><th>Used Limits</th><th>Remaining Limits</th><th>Time Frame</th><th>Date of Next Service</th></tr></thead><tbody>";
        for (var i = 0; i < MEServiceLimitationsSliced.length; i++) {

            var MESLProcedureCode = '';
            var MESLServiceLimitDescription = '';
            var MESLBenefitDescription = '';
            var MESLTotalLimits = '';
            var MESLUsedLimits = '';
            var MESLRemainingLimits = '';
            var MESLTimeframe = '';
            var MESLDateOfNextService = '';

            if (MEServiceLimitationsSliced[i].ProcedureCode != null) {
                MESLProcedureCode = MEServiceLimitationsSliced[i].ProcedureCode;
            }
            if (MEServiceLimitationsSliced[i].ServiceLimitDescription != null) {
                MESLServiceLimitDescription = MEServiceLimitationsSliced[i].ServiceLimitDescription;
            }
            if (MEServiceLimitationsSliced[i].BenefitDescription != null) {
                MESLBenefitDescription = MEServiceLimitationsSliced[i].BenefitDescription;
            }
            if (MEServiceLimitationsSliced[i].TotalLimits != null) {
                MESLTotalLimits = MEServiceLimitationsSliced[i].TotalLimits;
            }
            if (MEServiceLimitationsSliced[i].UsedLimits != null) {
                MESLUsedLimits = MEServiceLimitationsSliced[i].UsedLimits;
            }
            if (MEServiceLimitationsSliced[i].RemainingLimits != null) {
                MESLRemainingLimits = MEServiceLimitationsSliced[i].RemainingLimits;
            }
            if (MEServiceLimitationsSliced[i].Timeframe != null) {
                MESLTimeframe = MEServiceLimitationsSliced[i].Timeframe;
            }
            if (MEServiceLimitationsSliced[i].DateOfNextService != null) {
                MESLDateOfNextService = getFormattedDate(MEServiceLimitationsSliced[i].DateOfNextService.split('T')[0]);
            }

            table = table + "<tr><td>" + MESLProcedureCode + "</td><td>" + MESLServiceLimitDescription + "</td><td>" + MESLBenefitDescription + "</td><td>" + MESLTotalLimits + "</td><td>" + MESLUsedLimits + "</td><td>" + MESLRemainingLimits + "</td><td>" + MESLTimeframe + "</td><td>" + MESLDateOfNextService + "</td></tr>";
        }

        table = table + "</tbody></table>";
        document.getElementById('pnlResults11').innerHTML = '';
        document.getElementById('pnlResults11').innerHTML = table;

        if (MEPanelLet11.length > pageSize)
            setupPagination11(totalPages, pagenumber);
    }

    function panel12Refresh(pagenumber) {
        var startIndex = (pagenumber - 1) * pageSize;
        var endIndex = startIndex + pageSize;
        var MERestrictedCoveragesSliced = MEPanelLet12.slice(startIndex, endIndex);
        var totalPages = Math.ceil(MEPanelLet12.length / pageSize);
        var table = "<table id='paneltbl12' class='table csTable'><thead><tr><th>Effective Date</th><th>End Date</th></tr></thead><tbody>";
        for (var i = 0; i < MERestrictedCoveragesSliced.length; i++) {
            var MERCEffectiveDate = '';
            var MERCEndDate = '';

            if (MERestrictedCoveragesSliced[i].EffectiveDate != null) {
                MERCEffectiveDate = getFormattedDate(MERestrictedCoveragesSliced[i].EffectiveDate.split('T')[0]);
            }
            if (MERestrictedCoveragesSliced[i].EndDate != null) {
                MERCEndDate = getFormattedDate(MERestrictedCoveragesSliced[i].EndDate.split('T')[0]);
            }

            table = table + "<tr><td>" + MERCEffectiveDate + "</td><td>" + MERCEndDate + "</td></tr>";
        }

        table = table + "</tbody></table>";
        document.getElementById('pnlResults12').innerHTML = '';
        document.getElementById('pnlResults12').innerHTML = table;

        if (MEPanelLet12.length > pageSize)
            setupPagination12(totalPages, pagenumber);
    }

    function panel13Refresh(pagenumber) {
        var startIndex = (pagenumber - 1) * pageSize;
        var endIndex = startIndex + pageSize;
        var MEUnder19FamilyMembersSliced = MEPanelLet13.slice(startIndex, endIndex);
        var totalPages = Math.ceil(MEPanelLet13.length / pageSize);
        var table = "<table id='paneltbl13' class='table csTable'><thead><tr><th>Medicaid Billing Number</th><th>First Name</th><th>MI</th><th>Last Name</th><th>Gender</th><th>Date of Birth</th></tr></thead><tbody>";
        for (var i = 0; i < MEUnder19FamilyMembersSliced.length; i++) {
            var MEMedicaidId = '';
            var MEUU9DateOfBirth = '';
            var MEFirstName = '';
            var MEMiddleInitial = '';
            var MELastName = '';
            var MEGender = '';

            if (MEUnder19FamilyMembersSliced[i].MedicaidId != null) {
                MEMedicaidId = MEUnder19FamilyMembersSliced[i].MedicaidId;
            }
            if (MEUnder19FamilyMembersSliced[i].DateOfBirth != null) {
                MEUU9DateOfBirth = getFormattedDate(MEUnder19FamilyMembersSliced[i].DateOfBirth.split('T')[0]);
            }
            if (MEUnder19FamilyMembersSliced[i].FirstName != null) {
                MEFirstName = MEUnder19FamilyMembersSliced[i].FirstName;
            }
            if (MEUnder19FamilyMembersSliced[i].MiddleInitial != null) {
                MEMiddleInitial = MEUnder19FamilyMembersSliced[i].MiddleInitial;
            }
            if (MEUnder19FamilyMembersSliced[i].LastName != null) {
                MELastName = MEUnder19FamilyMembersSliced[i].LastName;
            }
            if (MEUnder19FamilyMembersSliced[i].Gender != null) {
                MEGender = MEUnder19FamilyMembersSliced[i].Gender;
            }

            table = table + "<tr><td>" + MEMedicaidId + "</td><td>" + MEFirstName + "</td><td>" + MEMiddleInitial + "</td><td>" + MELastName + "</td><td>" + MEGender + "</td><td>" + MEUU9DateOfBirth + "</td></tr>";
        }

        table = table + "</tbody></table>";
        document.getElementById('pnlResults13').innerHTML = '';
        document.getElementById('pnlResults13').innerHTML = table;

        if (MEPanelLet13.length > pageSize)
            setupPagination13(totalPages, pagenumber);
    }

    function setupPagination3(totalPages, currentPage) {
        var paginationDiv = document.getElementById('pagination3');

        paginationDiv.innerHTML = '';

        var buttonLis = "<button type='button' disabled id='btnPrev3' onclick='PreviousClick3(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";

        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage3_" + i + "' class='mybtnpage3' type='button' onclick='btnPaging3_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "<button type='button' id='btnNext3' onclick='NextClick3(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
        paginationDiv.innerHTML = buttonLis;

        var pageId = 'btnPage3_' + currentPage;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'

        var btnPrev3 = document.getElementById('btnPrev3');

        if (currentPage === 1) {
            $("#btnPrev3").prop('disabled', true);
        }
        else {
            $("#btnPrev3").prop('disabled', false);
        }

        if (currentPage === totalPages) {
            $("#btnNext3").prop('disabled', true);
        }
        else {
            $("#btnNext3").prop('disabled', false);
        }
    }


    function setupPagination4(totalPages, currentPage) {
        var paginationDiv = document.getElementById('pagination4');

        paginationDiv.innerHTML = '';

        var buttonLis = "<button type='button' disabled id='btnPrev4' onclick='PreviousClick4(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";

        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage4_" + i + "' class='mybtnpage4' type='button' onclick='btnPaging4_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "<button type='button' id='btnNext4' onclick='NextClick4(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
        paginationDiv.innerHTML = buttonLis;

        var pageId = 'btnPage4_' + currentPage;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'

        var btnPrev4 = document.getElementById('btnPrev4');

        if (currentPage === 1) {
            $("#btnPrev4").prop('disabled', true);
        }
        else {
            $("#btnPrev4").prop('disabled', false);
        }

        if (currentPage === totalPages) {
            $("#btnNext4").prop('disabled', true);
        }
        else {
            $("#btnNext4").prop('disabled', false);
        }
    }

    function setupPagination5(totalPages, currentPage) {
        var paginationDiv = document.getElementById('pagination5');

        paginationDiv.innerHTML = '';

        var buttonLis = "<button type='button' disabled id='btnPrev5' onclick='PreviousClick5(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";

        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage5_" + i + "' class='mybtnpage5' type='button' onclick='btnPaging5_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "<button type='button' id='btnNext5' onclick='NextClick5(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
        paginationDiv.innerHTML = buttonLis;

        var pageId = 'btnPage5_' + currentPage;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'

        var btnPrev5 = document.getElementById('btnPrev5');

        if (currentPage === 1) {
            $("#btnPrev5").prop('disabled', true);
        }
        else {
            $("#btnPrev5").prop('disabled', false);
        }

        if (currentPage === totalPages) {
            $("#btnNext5").prop('disabled', true);
        }
        else {
            $("#btnNext5").prop('disabled', false);
        }
    }

    function setupPagination6(totalPages, currentPage) {
        var paginationDiv = document.getElementById('pagination6');

        paginationDiv.innerHTML = '';

        var buttonLis = "<button type='button' disabled id='btnPrev6' onclick='PreviousClick6(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";

        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage6_" + i + "' class='mybtnpage6' type='button' onclick='btnPaging6_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "<button type='button' id='btnNext6' onclick='NextClick6(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
        paginationDiv.innerHTML = buttonLis;

        var pageId = 'btnPage6_' + currentPage;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'

        var btnPrev6 = document.getElementById('btnPrev6');

        if (currentPage === 1) {
            $("#btnPrev6").prop('disabled', true);
        }
        else {
            $("#btnPrev6").prop('disabled', false);
        }

        if (currentPage === totalPages) {
            $("#btnNext6").prop('disabled', true);
        }
        else {
            $("#btnNext6").prop('disabled', false);
        }
    }

    function setupPagination7(totalPages, currentPage) {
        var paginationDiv = document.getElementById('pagination7');

        paginationDiv.innerHTML = '';

        var buttonLis = "<button type='button' disabled id='btnPrev7' onclick='PreviousClick7(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";

        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage7_" + i + "' class='mybtnpage7' type='button' onclick='btnPaging7_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "<button type='button' id='btnNext7' onclick='NextClick7(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
        paginationDiv.innerHTML = buttonLis;

        var pageId = 'btnPage7_' + currentPage;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'

        var btnPrev7 = document.getElementById('btnPrev7');

        if (currentPage === 1) {
            $("#btnPrev7").prop('disabled', true);
        }
        else {
            $("#btnPrev7").prop('disabled', false);
        }

        if (currentPage === totalPages) {
            $("#btnNext7").prop('disabled', true);
        }
        else {
            $("#btnNext7").prop('disabled', false);
        }
    }

    function setupPagination8(totalPages, currentPage) {
        var paginationDiv = document.getElementById('pagination8');

        paginationDiv.innerHTML = '';

        var buttonLis = "<button type='button' disabled id='btnPrev8' onclick='PreviousClick8(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";

        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage8_" + i + "' class='mybtnpage8' type='button' onclick='btnPaging8_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "<button type='button' id='btnNext8' onclick='NextClick8(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
        paginationDiv.innerHTML = buttonLis;

        var pageId = 'btnPage8_' + currentPage;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'

        var btnPrev8 = document.getElementById('btnPrev8');

        if (currentPage === 1) {
            $("#btnPrev8").prop('disabled', true);
        }
        else {
            $("#btnPrev8").prop('disabled', false);
        }

        if (currentPage === totalPages) {
            $("#btnNext8").prop('disabled', true);
        }
        else {
            $("#btnNext8").prop('disabled', false);
        }
    }

    function setupPagination9(totalPages, currentPage) {
        var paginationDiv = document.getElementById('pagination9');

        paginationDiv.innerHTML = '';

        var buttonLis = "<button type='button' disabled id='btnPrev9' onclick='PreviousClick9(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";

        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage9_" + i + "' class='mybtnpage9' type='button' onclick='btnPaging9_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "<button type='button' id='btnNext9' onclick='NextClick9(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
        paginationDiv.innerHTML = buttonLis;

        var pageId = 'btnPage9_' + currentPage;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'

        var btnPrev9 = document.getElementById('btnPrev9');

        if (currentPage === 1) {
            $("#btnPrev9").prop('disabled', true);
        }
        else {
            $("#btnPrev9").prop('disabled', false);
        }

        if (currentPage === totalPages) {
            $("#btnNext9").prop('disabled', true);
        }
        else {
            $("#btnNext9").prop('disabled', false);
        }
    }

    function setupPagination10(totalPages, currentPage) {
        var paginationDiv = document.getElementById('pagination10');

        paginationDiv.innerHTML = '';

        var buttonLis = "<button type='button' disabled id='btnPrev10' onclick='PreviousClick10(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";

        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage10_" + i + "' class='mybtnpage10' type='button' onclick='btnPaging10_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "<button type='button' id='btnNext10' onclick='NextClick10(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
        paginationDiv.innerHTML = buttonLis;

        var pageId = 'btnPage10_' + currentPage;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'

        var btnPrev10 = document.getElementById('btnPrev10');

        if (currentPage === 1) {
            $("#btnPrev10").prop('disabled', true);
        }
        else {
            $("#btnPrev10").prop('disabled', false);
        }

        if (currentPage === totalPages) {
            $("#btnNext10").prop('disabled', true);
        }
        else {
            $("#btnNext10").prop('disabled', false);
        }
    }

    function setupPagination11(totalPages, currentPage) {
        var paginationDiv = document.getElementById('pagination11');

        paginationDiv.innerHTML = '';

        var buttonLis = "<button type='button' disabled id='btnPrev11' onclick='PreviousClick11(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";

        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage11_" + i + "' class='mybtnpage11' type='button' onclick='btnPaging11_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "<button type='button' id='btnNext11' onclick='NextClick11(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
        paginationDiv.innerHTML = buttonLis;

        var pageId = 'btnPage11_' + currentPage;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'

        var btnPrev11 = document.getElementById('btnPrev11');

        if (currentPage === 1) {
            $("#btnPrev11").prop('disabled', true);
        }
        else {
            $("#btnPrev11").prop('disabled', false);
        }

        if (currentPage === totalPages) {
            $("#btnNext11").prop('disabled', true);
        }
        else {
            $("#btnNext11").prop('disabled', false);
        }
    }

    function setupPagination12(totalPages, currentPage) {
        var paginationDiv = document.getElementById('pagination12');

        paginationDiv.innerHTML = '';

        var buttonLis = "<button type='button' disabled id='btnPrev12' onclick='PreviousClick12(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";

        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage12_" + i + "' class='mybtnpage12' type='button' onclick='btnPaging12_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "<button type='button' id='btnNext12' onclick='NextClick12(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
        paginationDiv.innerHTML = buttonLis;

        var pageId = 'btnPage12_' + currentPage;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'

        var btnPrev12 = document.getElementById('btnPrev12');

        if (currentPage === 1) {
            $("#btnPrev12").prop('disabled', true);
        }
        else {
            $("#btnPrev12").prop('disabled', false);
        }

        if (currentPage === totalPages) {
            $("#btnNext12").prop('disabled', true);
        }
        else {
            $("#btnNext12").prop('disabled', false);
        }
    }

    function setupPagination13(totalPages, currentPage) {
        var paginationDiv = document.getElementById('pagination13');

        paginationDiv.innerHTML = '';

        var buttonLis = "<button type='button' disabled id='btnPrev13' onclick='PreviousClick13(" + totalPages + "," + currentPage + ")' class='disabled'>Previous</button>";

        for (let i = 1; i <= totalPages; i++) {
            buttonLis = buttonLis + "<button id='btnPage13_" + i + "' class='mybtnpage13' type='button' onclick='btnPaging13_Click(" + i + ")' title='" + i + "'>" + i + "</button>";
        }
        buttonLis = buttonLis + "<button type='button' id='btnNext13' onclick='NextClick13(" + totalPages + "," + currentPage + ")' class='disabled'>Next</button>";
        paginationDiv.innerHTML = buttonLis;

        var pageId = 'btnPage13_' + currentPage;
        var paginationDiv = document.getElementById(pageId);
        paginationDiv.style.fontWeight = 'bold'

        var btnPrev13 = document.getElementById('btnPrev13');

        if (currentPage === 1) {
            $("#btnPrev13").prop('disabled', true);
        }
        else {
            $("#btnPrev13").prop('disabled', false);
        }

        if (currentPage === totalPages) {
            $("#btnNext13").prop('disabled', true);
        }
        else {
            $("#btnNext13").prop('disabled', false);
        }
    }


    function sortTable3(n) {        var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;        table = document.getElementById("paneltbl3");        switching = true;        dir = "asc"; // Set the sorting direction to ascending        // Loop to keep switching until no switching is needed        while (switching) {            switching = false;            rows = table.rows;
            for (i = 1; i < (rows.length - 1); i++) {                shouldSwitch = false;                x = rows[i].getElementsByTagName("TD")[n];                y = rows[i + 1].getElementsByTagName("TD")[n];                if (dir == "asc") {                    if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {                        shouldSwitch = true;                        break;                    }                } else if (dir == "desc") {                    if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {                        shouldSwitch = true;                        break;                    }                }            }
            if (shouldSwitch) {                rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);                switching = true;                switchcount++;            } else {                if (switchcount == 0 && dir == "asc") {                    dir = "desc";                    switching = true;                }            }        }        updateIcons3(n, dir);    }

    function sortTable4(n) {        var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;        table = document.getElementById("paneltbl4");        switching = true;        dir = "asc"; // Set the sorting direction to ascending        // Loop to keep switching until no switching is needed        while (switching) {            switching = false;            rows = table.rows;
            for (i = 1; i < (rows.length - 1); i++) {                shouldSwitch = false;                x = rows[i].getElementsByTagName("TD")[n];                y = rows[i + 1].getElementsByTagName("TD")[n];                if (dir == "asc") {                    if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {                        shouldSwitch = true;                        break;                    }                } else if (dir == "desc") {                    if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {                        shouldSwitch = true;                        break;                    }                }            }
            if (shouldSwitch) {                rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);                switching = true;                switchcount++;            } else {                if (switchcount == 0 && dir == "asc") {                    dir = "desc";                    switching = true;                }            }        }        updateIcons4(n, dir);    }

    function sortTable6(n) {        var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;        table = document.getElementById("paneltbl6");        switching = true;        dir = "asc"; // Set the sorting direction to ascending        // Loop to keep switching until no switching is needed        while (switching) {            switching = false;            rows = table.rows;
            for (i = 1; i < (rows.length - 1); i++) {                shouldSwitch = false;                x = rows[i].getElementsByTagName("TD")[n];                y = rows[i + 1].getElementsByTagName("TD")[n];                if (dir == "asc") {                    if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {                        shouldSwitch = true;                        break;                    }                } else if (dir == "desc") {                    if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {                        shouldSwitch = true;                        break;                    }                }            }
            if (shouldSwitch) {                rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);                switching = true;                switchcount++;            } else {                if (switchcount == 0 && dir == "asc") {                    dir = "desc";                    switching = true;                }            }        }        updateIcons6(n, dir);    }

    function sortTable9(n) {        var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;        table = document.getElementById("paneltbl9");        switching = true;        dir = "asc"; // Set the sorting direction to ascending        // Loop to keep switching until no switching is needed        while (switching) {            switching = false;            rows = table.rows;
            for (i = 1; i < (rows.length - 1); i++) {                shouldSwitch = false;                x = rows[i].getElementsByTagName("TD")[n];                y = rows[i + 1].getElementsByTagName("TD")[n];                if (dir == "asc") {                    if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {                        shouldSwitch = true;                        break;                    }                } else if (dir == "desc") {                    if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {                        shouldSwitch = true;                        break;                    }                }            }
            if (shouldSwitch) {                rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);                switching = true;                switchcount++;            } else {                if (switchcount == 0 && dir == "asc") {                    dir = "desc";                    switching = true;                }            }        }        updateIcons9(n, dir);    }

    function updateIcons3(colIndex, direction) {

        const icons = document.querySelectorAll('.sort-icon3');
        icons.forEach(icon => icon.innerHTML = '&#8597;');
        const icon = document.getElementById(`icon-${colIndex}`);
        if (direction === 'asc') {
            icon.innerHTML = '&#8593;'; // Up arrow
        } else {
            icon.innerHTML = '&#8595;'; // Down arrow
        }
    }

    function updateIcons4(colIndex, direction) {

        const icons = document.querySelectorAll('.sort-icon4');
        icons.forEach(icon => icon.innerHTML = '&#8597;');
        const icon = document.getElementById(`icon-${colIndex}`);
        if (direction === 'asc') {
            icon.innerHTML = '&#8593;'; // Up arrow
        } else {
            icon.innerHTML = '&#8595;'; // Down arrow
        }
    }

    function updateIcons6(colIndex, direction) {

        const icons = document.querySelectorAll('.sort-icon6');
        icons.forEach(icon => icon.innerHTML = '&#8597;');
        const icon = document.getElementById(`icon-${colIndex}`);
        if (direction === 'asc') {
            icon.innerHTML = '&#8593;'; // Up arrow
        } else {
            icon.innerHTML = '&#8595;'; // Down arrow
        }
    }

    function updateIcons9(colIndex, direction) {

        const icons = document.querySelectorAll('.sort-icon7');
        icons.forEach(icon => icon.innerHTML = '&#8597;');
        const icon = document.getElementById(`icon-${colIndex}`);
        if (direction === 'asc') {
            icon.innerHTML = '&#8593;'; // Up arrow
        } else {
            icon.innerHTML = '&#8595;'; // Down arrow
        }
    }

    function validateBillingNumber() {
        const numbers = /^[0-9]*$/;

        document.getElementById('txtMBN').classList.remove('errMsgInput');
        document.getElementById('spantxtMBN').innerHTML = "";

        var txtMedicaidBillingNumber = document.getElementById('txtMBN').value;

        if (txtMedicaidBillingNumber == null || txtMedicaidBillingNumber == undefined || txtMedicaidBillingNumber == '') {
            return true;
        }
        else {
            if (!numbers.test(txtMedicaidBillingNumber)) {
                document.getElementById('txtMBN').classList.add('errMsgInput');
                document.getElementById('spantxtMBN').innerHTML = "Please enter valid Medical billing number.";
                return false;
            }
            else if (txtMedicaidBillingNumber.length < 12) {
                document.getElementById('txtMBN').classList.add('errMsgInput');
                document.getElementById('spantxtMBN').innerHTML = "* 12-digit number is required.";
                return false;
            }

        }
    }

</script>

<asp:HiddenField ID="hdnUserName" runat="server" />
<asp:HiddenField ID="hdnMedicaidID" runat="server" />
<asp:HiddenField ID="hdnSearchDateTime" runat="server" />

<!-- #region ELIGIBILITY SEARCH -->
<div class="claimTitle csresult1" onclick="resultToggle1()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">ELIGIBILITY SEARCH</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer1 active">
    <div class="content cForm">
        <div id="pnlMEResults" style="overflow: auto; width: 100%"></div>
        <div><span tabindex="0" style="color: #CC0505; font-size: 14pt !important; padding-left: 10px; font-weight: 100 !important;">An asterisk * indicates a required field</span></div>
        <div class="form-horizontal col-md-6 col-centered">
            <div>
                <label class="control-label" for="txtMBN"><span style="color: red;">*</span> Medicaid Billing Number</label>
                <input class="form-control" id="txtMBN" name="Medicaid Billing Number" type="number" value="">
                <small class="errMsg" id="spantxtMBN"></small>
            </div>
        </div>
        <div class="form-horizontal col-md-6 col-centered">
            <div>
                <label class="control-label" for="txtSSN"><span style="color: red;">*</span> or SSN Example(123456789) No Dashes Needed</label>
                <input class="form-control" id="txtSSN" name="SSN" type="number" value="">
                <small class="errMsg" id="spantxtSSN"></small>
            </div>
        </div>
        <div class="form-horizontal col-md-6 col-centered">
            <div>
                <label class="control-label" for="txtDOB"><span style="color: red;">*</span> Date of Birth  Example(12/31/2025) Full Date Needed</label>
                <input id="txtDOB" name="Date of Birth" type="text" class="icon-rtl form-control" onblur="formatDatewithZero(this)" placeholder="mm/dd/yyyy">
                <small class="errMsg" id="spantxtDOB"></small>
            </div>
        </div>
        <div class="form-horizontal col-md-6 col-centered">
            <div>
                <label class="control-label" for="txtProcCode">Procedure Code</label>
                <input class="form-control" id="txtProcCode" name="Procedure Code" type="text" value="">
                <small class="errMsg" id="spantxtProcCode"></small>
            </div>
        </div>
        <div class="form-horizontal col-md-6 col-centered">
            <div>
                <label class="control-label" for="txtFDOS"><span style="color: red;">*</span> From DOS  Example(12/31/2025) Full Date Needed</label>
                <input id="txtFDOS" name="From DOS" type="text" class="icon-rtl form-control" onblur="formatDatewithZero(this)" placeholder="mm/dd/yyyy">
                <small class="errMsg" id="spantxtFDOS"></small>
            </div>
        </div>
        <div class="form-horizontal col-md-6 col-centered">
            <div>
                <label class="control-label" for="txtTDOS"><span style="color: red;">*</span> To DOS  Example(12/31/2025) Full Date Needed</label>
                <input id="txtTDOS" name="To DOS" type="text" class="icon-rtl form-control" onblur="formatDatewithZero(this)" placeholder="mm/dd/yyyy">
                <small class="errMsg" id="spantxtTDOS"></small>
            </div>
        </div>
        <div class="row" style="padding-top: 30px; text-align: center;">
            <div class="row-centered col-lg-12" style="padding-top: 30px; text-align: center;">
                <button id="btnSearch" type="button" class="btn btn-primary margin-right-n btn-success mebtn" style="background-color: #4a7b4d !important; min-width: 200px;" onclick="makeMemberEligibilityPageRequest()">Search</button>
                <button id="btnClear" type="button" class="btn btn-primary margin-right-n btn-danger clearme" style="background-color: #d03a3a !important; min-width: 200px;" onclick="clearForm()">Clear</button>
                <button id="btnPrint" type="button" class="btn btn-primary margin-right-n btn-success printme" style="background-color: #286090 !important; min-width: 200px; display: none" onclick="makePrintPageRequest()">Print</button>
            </div>
        </div>
    </div>
</div>
<!-- #endregion -->

<!-- #region RECIPIENT INFORMATION -->
<div class="claimTitle csresult2" onclick="resultToggle2()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">RECIPIENT INFORMATION</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer2">
    <div class="content cForm">
        <div class="form-horizontal col-md-4 col-centered">
            <div>
                <label class="control-label" for="txtMBNDisp">Medicaid Billing Number:</label>
                <input class="dark-input" id="txtMBNDisp" name="Medicaid Billing Number" type="text" value="" disabled>
            </div>
        </div>
        <div class="form-horizontal col-md-4 col-centered">
            <div>
                <label class="control-label" for="txtDOBDisp">Date of Birth:</label>
                <input class="dark-input" id="txtDOBDisp" name="Date of Birth" type="text" value="" disabled>
            </div>
        </div>
        <div class="form-horizontal col-md-4 col-centered">
            <div>
                <label class="control-label" for="txtCORDisp">County of Residence:</label>
                <input class="dark-input" id="txtCORDisp" name="County of Residence" type="text" value="" disabled>
            </div>
        </div>
        <div class="form-horizontal col-md-4 col-centered">
            <div>
                <label class="control-label" for="txtLNDisp">Last Name:</label>
                <input class="dark-input" id="txtLNDisp" name="Last Name" type="text" value="" disabled>
            </div>
        </div>
        <div class="form-horizontal col-md-4 col-centered">
            <div>
                <label class="control-label" for="txtDODDisp">Date Of Death:</label>
                <input class="dark-input" id="txtDODDisp" name="Date Of Death" type="text" value="" disabled>
            </div>
        </div>
        <div class="form-horizontal col-md-4 col-centered">
            <div>
                <label class="control-label" for="txtCOEDisp">County of Eligibility:</label>
                <input class="dark-input" id="txtCOEDisp" name="County of Eligibility" type="text" value="" disabled>
            </div>
        </div>
        <div class="form-horizontal col-md-4 col-centered">
            <div>
                <label class="control-label" for="txtFNLNDisp">First Name, MI:</label>
                <input class="dark-input" id="txtFNLNDisp" name="First Name, MI" type="text" value="" disabled>
            </div>
        </div>
        <div class="form-horizontal col-md-4 col-centered">
            <div>
                <label class="control-label" for="txtSSNDisp">SSN:</label>
                <input class="dark-input" id="txtSSNDisp" name="SSN" type="text" value="" disabled>
            </div>
        </div>
        <div class="form-horizontal col-md-4 col-centered">
            <div>
                <label class="control-label" for="txtCOIDisp">County Office Information:</label>
                <a id="txtCOIDisp" name="County Office Information" href="https://jfs.ohio.gov/about/local-agencies-directory" style="height: 44px; display: block; font-size: 22px; line-height: 1.1; padding-top: 8px;">https://jfs.ohio.gov/about/local-agencies-directory</a>
            </div>
        </div>
        <div class="form-horizontal col-md-4 col-centered">
            <div>
                <label class="control-label" for="txtGenderDisp">Gender:</label>
                <input class="dark-input" id="txtGenderDisp" name="Gender" type="text" value="" disabled>
            </div>
        </div>
    </div>
</div>
<!-- #endregion -->

<!-- BENEFIT/ASSIGNMENT PLAN(S) -->
<div class="claimTitle csresult3" onclick="resultToggle3()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">BENEFIT/ASSIGNMENT PLAN(S)</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer3">
    <div id="pnlResults3" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
    <div id="pagination3" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
</div>

<!-- MANAGED CARE PLANS -->
<div class="claimTitle csresult4" onclick="resultToggle4()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">MANAGED CARE PLANS</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer4">
    <div id="pnlResults4" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
    <div id="pagination4" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
</div>

<!-- THIRD PARTY LIABILITY -->
<div class="claimTitle csresult5" onclick="resultToggle5()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">THIRD PARTY LIABILITY</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer5">
    <div id="pnlResults5" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
    <div id="pagination5" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
</div>

<!-- PATIENT LIABILITY -->
<div class="claimTitle csresult6" onclick="resultToggle6()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">PATIENT LIABILITY</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer6">
    <div id="pnlResults6" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
    <div id="pagination6" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
</div>

<!-- LONG TERM CARE FACILITY PLACEMENTS -->
<div class="claimTitle csresult7" onclick="resultToggle7()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">LONG TERM CARE FACILITY PLACEMENTS</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer7">
    <div id="pnlResults7" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
    <div id="pagination7" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
</div>

<!-- LOCK IN -->
<div class="claimTitle csresult8" onclick="resultToggle8()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">LOCK IN</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer8">
    <div id="pnlResults8" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
    <div id="pagination8" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
</div>

<div class="claimTitle csresult9" onclick="resultToggle9()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">MEDICARE</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer9">
    <div id="pnlResults9" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
    <div id="pagination9" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
</div>

<div class="claimTitle csresult11" onclick="resultToggle11()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">SERVICE LIMITATION</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer11">
    <div id="pnlResults11" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
    <div id="pagination11" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
</div>

<div class="claimTitle csresult12" onclick="resultToggle12()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">RESTRICTED COVERAGE</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer12">
    <div id="pnlResults12" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
    <div id="pagination12" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
</div>

<div class="claimTitle csresult13" onclick="resultToggle13()">
    <div class="cttile">
        <h3 class="pl-2" style="font-weight: bold">ASSOCIATED CHILD(REN)</h3>
    </div>
    <div>
        <span class="plus">+</span> <span class="minus">-</span>
    </div>
</div>
<div class="resultContainer13">
    <div id="pnlResults13" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
    <div id="pagination13" style="overflow: auto; width: 100%; background-color: #FFF;"></div>
</div>
