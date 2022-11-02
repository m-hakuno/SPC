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


Partial Public Class BPIFIXP001

    '''<summary>
    '''lblTboxId1 コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents lblTboxId1 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''lblTboxId2 コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents lblTboxId2 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''lblDifferenceY コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents lblDifferenceY As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''lblCountY コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents lblCountY As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''lblCountUnitY コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents lblCountUnitY As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''lblHallNm1 コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents lblHallNm1 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''lblHallNm2 コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents lblHallNm2 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''lblDifferenceN コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents lblDifferenceN As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''lblCountN コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents lblCountN As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''lblCountUnitN コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents lblCountUnitN As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''lblVer1 コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents lblVer1 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''lblVer2 コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents lblVer2 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''DivOut コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents DivOut As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''DivIn コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents DivIn As Global.System.Web.UI.HtmlControls.HtmlGenericControl

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
    Public Shadows ReadOnly Property Master() As SPC.reference
        Get
            Return CType(MyBase.Master, SPC.reference)
        End Get
    End Property
End Class
