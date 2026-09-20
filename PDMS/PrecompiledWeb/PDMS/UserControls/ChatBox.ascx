<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ChatBox, App_Web_p4ixifjm" %>

<style type="text/css">
    .drop_menu li
    {
        font: 13px/1.231 arial,helvetica,clean,sans-serif;
        text-shadow: none;
        *font-size: small;
        *font: x-small;
    }

    table
    {
        font-size: inherit;
        font: 100%;
    }

    pre, code, kbd, samp, tt
    {
        font-family: monospace;
        *font-size: 108%;
        line-height: 100%;
    }

    .chatbox
    {
        color: white;
        z-index: 99;
        position: fixed;
        margin: 0px;
        bottom: -36px;
        right: 6%;
        height: 80px;
        background-color: #333;
        font-family: 'Open Sans Condensed', sans-serif;
        text-decoration: none;
        font-size: 20px;
        font-weight: bold;
        width: 180px;
        padding-left: 20px;
        padding-top: 8px;
        background: #00abcf;
        background: -moz-linear-gradient(top, #00abcf 75%, #0290ae 100%);
        background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#00abcf), color-stop(100%,#0290ae));
        background: -webkit-linear-gradient(top, #00abcf 75%,#0290ae 100%);
        background: -o-linear-gradient(top, #00abcf 75%,#0290ae 100%);
        background: -ms-linear-gradient(top, #00abcf 75%,#0290ae 100%);
        background: linear-gradient(to bottom, #00abcf 75%,#0290ae 100%);
        filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#00abcf', endColorstr='#0290ae',GradientType=0 );
        -moz-border-radius: 2px;
        -webkit-border-radius: 2px;
        -khtml-border-radius: 2px;
        border-radius: 2px;
        transition: all .2s;
        -moz-transition: all .2s;
        -webkit-transition: all .2s;
        -o-transition: all .2s;
    }

        .chatbox img
        {
            position: absolute;
            right: 18px;
            top: 8px;
        }

    .chatdesc
    {
        font-family: 'Open Sans Condensed', sans-serif;
        font-weight: normal;
        font-size: 10px;
        text-align: center;
        left: 20px;
        top: 40px;
        position: absolute;
        color: white;
        letter-spacing: 1px;
    }

    .chatbox:hover
    {
        background: #c85639;
        -webkit-transform: translate(0,-30px);
        -moz-transform: translate(0,-30px);
        -o-transform: translate(0,-30px);
        transform: translate(0,-30px);
        color: #FFFFFF;
        cursor: pointer;
    }
</style>

<%--   <script type="text/javascript" src="https://widget-ui.medchatapp.com/v1/widget.js?api-key=Z1eh9IDvtkyW9lgwAu2EyA"></script>--%>

<script type="text/javascript" async src="https://medchatapp.com/widget/widget.js?api-key=lv-R89GjakeXREC6XXrdVw"></script>
