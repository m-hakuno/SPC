'------------------------------------------------------------------------------
' <自動生成>
'     このコードはツールによって生成されました。
'
'     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
'     コードが再生成されるときに損失したりします。 
' </自動生成>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class COMUPDM19

    '''<summary>
    '''txtSrch_TelNo コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents txtSrch_TelNo As Global.SPC.ClsCMTextBox

    '''<summary>
    '''ddlSrch_JudgeCls コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents ddlSrch_JudgeCls As Global.SPC.ClsCMDropDownList

    '''<summary>
    '''txtSrch_tboxidFromTo コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents txtSrch_tboxidFromTo As Global.SPC.ClsCMTextBoxFromTo

    '''<summary>
    '''txtSrch_Hall コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents txtSrch_Hall As Global.SPC.ClsCMTextBox

    '''<summary>
    '''ddlSrchOperate コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents ddlSrchOperate As Global.SPC.ClsCMDropDownList

    '''<summary>
    '''ddlDel コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents ddlDel As Global.SPC.ClsCMDropDownList

    '''<summary>
    '''txtTelNo コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents txtTelNo As Global.SPC.ClsCMTextBox

    '''<summary>
    '''txtOldTelNo コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents txtOldTelNo As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''ddlJudge_Cls コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents ddlJudge_Cls As Global.SPC.ClsCMDropDownList

    '''<summary>
    '''txtName コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents txtName As Global.SPC.ClsCMTextBox

    '''<summary>
    '''txtOldHallCD コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents txtOldHallCD As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''lblHall コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents lblHall As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''ddlCompCD コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents ddlCompCD As Global.SPC.ClsCMDropDownList

    '''<summary>
    '''ddlEmployee コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents ddlEmployee As Global.SPC.ClsCMDropDownList

    '''<summary>
    '''hdnData コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents hdnData As Global.System.Web.UI.HtmlControls.HtmlInputHidden

    '''<summary>
    '''grvList コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents grvList As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Master プロパティ。
    '''</summary>
    '''<remarks>
    '''自動生成されたプロパティ。
    '''</remarks>
    Public Shadows ReadOnly Property Master() As SPC.MstMaster
        Get
            Return CType(MyBase.Master, SPC.MstMaster)
        End Get
    End Property
End Class
