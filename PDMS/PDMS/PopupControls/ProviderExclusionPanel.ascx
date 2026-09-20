<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ProviderExclusionPanel" Codebehind="ProviderExclusionPanel.ascx.cs" %>
<style>
    :root {
        --bg: #0f172a;
        --panel: #111827;
        --panel-2: #0b1220;
        --text: #e5e7eb;
        --muted: #9ca3af;
        --accent: #60a5fa;
        --accent-2: #3b82f6;
        --danger: #ef4444;
        --warn: #f59e0b;
        --ok: #10b981;
        --chip: #1f2937;
        --border: #1f2937;
        --code: #0b1220;
        --shadow: 0 10px 30px rgba(0,0,0,.45), inset 0 1px 0 rgba(255,255,255,.03);
        --radius: 14px;
    }

    .px-container {
        max-width: 1000px;
        margin: 0 auto;
    }

    .px-panel {
        overflow: hidden;
        font-family: 'Noto Sans Regular',arial, helvetica, sans-serif !important;
    }

    .px-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 18px 22px;
        border-bottom: 1px solid var(--border);
    }

    .px-title {
        display: flex;
        gap: 14px;
        align-items: center;
    }

    .px-title__icon {
        width: 36px;
        height: 36px;
        display: grid;
        place-items: center;
        border-radius: 10px;
        background: radial-gradient(100% 100% at 50% 0%, #1e293b 0%, #0b1220 100%);
        color: #93c5fd;
        border: 1px solid #1f2a44;
    }

    .px-title__text {
        display: flex;
        flex-direction: column
    }

        .px-title__text h1 {
            font-size: 18px;
            margin: 0 0 2px 0;
            font-weight: 700;
            letter-spacing: .2px;
        }

    .px-title__meta {
        font-size: 12px;
    }

    .px-badges {
        display: flex;
        gap: 8px;
        flex-wrap: wrap;
    }

    .px-badge {
        font-size: 12px;
        padding: 6px 10px;
        border-radius: 999px;
        border: 1px solid var(--border);
        background: var(--chip);
        color: var(--text);
        display: inline-flex;
        gap: 6px;
        align-items: center;
    }

    .px-badge--status {
        border-color: #3f2a2a;
        background: #1a0f0f;
        color: #ffffff;
    }

    .dot {
        width: 8px;
        height: 8px;
        border-radius: 999px;
        background: var(--warn);
        display: inline-block;
    }

    .px-body {
        display: grid;
        grid-template-columns: 1.2fr 1fr;
    }

    @media (max-width:880px) {
        .px-body {
            grid-template-columns: 1fr;
        }
    }

    .px-section {
        padding: 18px 22px;
        border-right: 1px solid var(--border)
    }

        .px-section:last-child {
            border-right: none;
        }

        .px-section h2 {
            font-size: 14px;
            text-transform: uppercase;
            letter-spacing: .12em;
            color: #000;
            margin: 0 0 12px 0;
        }

    .px-grid {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 12px 18px;
    }

    @media (max-width:520px) {
        .px-grid {
            grid-template-columns: 1fr;
        }
    }

    .kv {
        display: flex;
        flex-direction: column;
        gap: 6px;
        background: rgba(255,255,255,.02);
        border: 1px solid var(--border);
        border-radius: 10px;
        padding: 10px 12px;
    }

        .kv label {
            font-size: 14px;
            letter-spacing: .08em;
            color: #4C4C4C;
        }

        .kv .value {
            font-size: 14px;
            color: #000;
            word-break: break-word;
        }

        .kv .value--empty {
            color: #6b7280;
            font-style: italic;
        }

    .note {
        margin-top: 14px;
        background: rgba(96,165,250,.08);
        border: 1px solid rgba(96,165,250,.25);
        color: #000;
        padding: 10px 12px;
        border-radius: 10px;
        font-size: 14px;
    }

    .meta-grid {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 12px 18px;
    }

    @media (max-width:520px) {
        .meta-grid {
            grid-template-columns: 1fr
        }
    }

    .legend {
        display: flex;
        gap: 10px;
        align-items: center;
        font-size: 12px;
        margin-top: 6px;
    }

        .legend .dot.danger {
            background: var(--danger);
        }
        
    .details {
        color: #000;
        border-radius: 10px;
        line-height: 1.5;
        font-size: 14px;
    }
</style>

<div class="px-panel" role="region" aria-labelledby="<%= this.ClientID %>_title">
    <header class="px-header">
        <div class="px-title">
            <div class="px-title__icon" aria-hidden="true">🧾</div>
            <div class="px-title__text">
                <h1 id="<%= this.ClientID %>_title">Provider Exclusion Summary</h1>
                <div class="px-title__meta">
                    Record type:
          <asp:Literal ID="litProviderClassification" runat="server" />
                    &middot;
          Source:
                    <asp:Literal ID="litExclusionAgency" runat="server" />
                </div>
            </div>
        </div>
        <div class="px-badges" aria-label="Status badges">
            <span class="px-badge px-badge--status" title="Exclusion status">
                <span class="dot"></span>
                Exclusion:
        <asp:Literal ID="litExclusionStatus" runat="server" />
            </span>
            <span class="px-badge" title="Program">
                <asp:Literal ID="litExclusionProgram" runat="server" /></span>
            <span class="px-badge" title="Agency">
                <asp:Literal ID="litExclusionAgencyBadge" runat="server" /></span>
        </div>
    </header>

    <section class="px-body">
        <div class="px-section" aria-label="Identity and Program Details">
            <h2>Identity &amp; Program</h2>
            <div class="px-grid">
                <div class="kv">
                    <label>Provider Classification</label>
                    <div class="value">
                        <asp:Literal ID="litProviderClassification2" runat="server" /></div>
                </div>
                <div class="kv">
                    <label>Organization Name</label>
                    <div class="value">
                        <asp:Literal ID="litOrganizationName" runat="server" /></div>
                </div>

                <div class="kv">
                    <label>First Name</label>
                    <div class="value">
                        <asp:Literal ID="litFirstName" runat="server" /></div>
                </div>
                <div class="kv">
                    <label>Middle Name</label>
                    <div class="value">
                        <asp:Literal ID="litMiddleName" runat="server" /></div>
                </div>

                <div class="kv">
                    <label>Last Name</label>
                    <div class="value">
                        <asp:Literal ID="litLastName" runat="server" /></div>
                </div>
                <div class="kv">
                    <label>Suffix</label>
                    <div class="value">
                        <asp:Literal ID="litSuffix" runat="server" /></div>
                </div>

                <div class="kv">
                    <label>SSN</label>
                    <div class="value">
                        <asp:Literal ID="litSSN" runat="server" /></div>
                </div>
                <div class="kv">
                    <label>EIN</label>
                    <div class="value">
                        <asp:Literal ID="litEIN" runat="server" /></div>
                </div>

                <div class="kv">
                    <label>Date of Birth (DOB)</label>
                    <div class="value">
                        <asp:Literal ID="litDOB" runat="server" /></div>
                </div>
                <div class="kv">
                    <label>NPI</label>
                    <div class="value">
                        <asp:Literal ID="litNPI" runat="server" /></div>
                </div>

                <div class="kv">
                    <label>Date of Death (DOD)</label>
                    <div class="value">
                        <asp:Literal ID="litDOD" runat="server" /></div>
                </div>
                <div class="kv">
                    <label>Exclusion Agency ID</label>
                    <div class="value">
                        <asp:Literal ID="litExclusionAgencyId" runat="server" /></div>
                </div>
            </div>

            <div class="note" role="note">
                Fields shown with <em>—</em> were not provided in the source data.
            </div>
        </div>

        <div class="px-section" aria-label="Exclusion Details">
            <h2>Exclusion Details</h2>
            <div class="meta-grid">
                <div class="kv">
                    <label>Exclusion Program</label>
                    <div class="value">
                        <asp:Literal ID="litExclusionProgram2" runat="server" /></div>
                </div>
                <div class="kv">
                    <label>Exclusion Agency</label>
                    <div class="value">
                        <asp:Literal ID="litExclusionAgency2" runat="server" /></div>
                </div>

                <div class="kv">
                    <label>Exclusion Type</label>
                    <div class="value">
                        <asp:Literal ID="litExclusionType" runat="server" /></div>
                </div>
                <div class="kv">
                    <label>Exclusion Date</label>
                    <div class="value">
                        <asp:Literal ID="litExclusionDate" runat="server" /></div>
                </div>

                <div class="kv">
                    <label>Exclusion Status</label>
                    <div class="value">
                        <asp:Literal ID="litExclusionStatus2" runat="server" /></div>
                </div>
                <div class="kv">
                    <label>Exclusion Termination Date</label>
                    <div class="value">
                        <asp:Literal ID="litExclusionTerminationDate" runat="server" /></div>
                </div>

                <div class="kv">
                    <label>Reinstatement Date</label>
                    <div class="value">
                        <asp:Literal ID="litReinstatementDate" runat="server" /></div>
                </div>
            </div>

            <div class="legend" aria-hidden="true">
                <span class="dot danger"></span>Active exclusion
            </div>

            <div style="height: 12px"></div>

            <div class="kv">
                <label>Additional Details</label>
                <div class="details">
                    <asp:Literal ID="litAdditionalDetails" runat="server" /></div>
            </div>
        </div>
    </section>
</div>
