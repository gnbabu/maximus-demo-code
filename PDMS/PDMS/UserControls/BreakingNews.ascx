<%@ Control Language="C#" AutoEventWireup="true" Inherits="UIControls_BreakingNews" Codebehind="BreakingNews.ascx.cs" %>

<!-- News Box -->
<br />
<div style="width:100%;">
<table class="gridView" style="margin:auto" >
    <thead>
        <tr class="gridViewHeader">
            <td>Latest News</td>
        </tr>
    </thead>
    <tbody>
        <tr class="gridViewRow">
            <td style="min-height:100px">
                <div ID="divNewsItems" enableviewstate="false" runat="server"></div>
            </td>
        </tr>
    </tbody>
</table>
</div>