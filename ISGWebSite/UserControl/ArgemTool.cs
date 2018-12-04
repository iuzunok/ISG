using System.Text;
using System.Web.Mvc;

namespace ISGWebSite.UserControl
{
    public static class ArgemTool
    {
        public static MvcHtmlString ArgemDDLText(this HtmlHelper oHtmlHelper, string Id,/* string ScopeAd,*/ string NgModel, string PlaceHolder, string Width = "250")
        {
            StringBuilder oSB = new StringBuilder();

            oSB.AppendLine("<div class=\"input-group ardt_container\">");
            oSB.AppendLine("<input type=\"text\" id=\"" + Id + "\" class=\"form-control ardt_input\" style=\"width:"+ Width + "px\" placeholder=\"" + PlaceHolder + "\">");
            oSB.AppendLine("<input type=\"hidden\" id=\"hdn" + Id + "\" ng-model=\"" + NgModel + "\" class=\"ardt_input\">");
            oSB.AppendLine("<div class=\"input-group-append\">");
            oSB.AppendLine("<button id=\"" + Id + "_VeriGetir\" class=\"btn btn-gri\" type=\"button\">");
            oSB.AppendLine("<i class=\"fa fa-search\"></i>");
            oSB.AppendLine("</button>");
            oSB.AppendLine("<button id=\"" + Id + "_VeriTemizle\" class=\"btn btn-gri ardt_clear_data\" type=\"button\">");
            oSB.AppendLine("<i class=\"fa fa-remove\"></i>");
            oSB.AppendLine("</button>");
            oSB.AppendLine("</div>");
            oSB.AppendLine("</div>");

            oSB.AppendLine("<script>");
            oSB.AppendLine("var " + Id + " = document.getElementById('" + Id + "');");

            // oSB.AppendLine(Id + ".ScopeAd = " + ScopeAd + ";");
            oSB.AppendLine(Id + ".EkPapametere = '';");
            oSB.AppendLine(Id + ".MinCharAdet = 1; ");
            oSB.AppendLine(Id + ".ParametreAd = 'AramaKriter';");
            oSB.AppendLine(Id + ".ControlName = '/Yetki/Kullanici/KullaniciGetirDDLText';");
            /*cph1_ucUnvan_input.Parametreler = {
                TabloAd: 'Yetki',
                AlanAd: 'KullaniciTipNo'
            };*/
            oSB.AppendLine(Id + ".HdnAlanId = 'hdn" + Id + "';");
            //var cph1_ucUnvan_cv = document.all ? document.all["cph1_ucUnvan_cv"] : document.getElementById("cph1_ucUnvan_cv");
            //cph1_ucUnvan_cv.controltovalidate = "cph1_ucUnvan_input";
            //cph1_ucUnvan_cv.focusOnError = "t";
            //cph1_ucUnvan_cv.display = "None";
            //cph1_ucUnvan_cv.evaluationfunction = "CustomValidatorEvaluateIsValid";
            //cph1_ucUnvan_cv.clientvalidationfunction = "ArgemDDLTextUtil.TextKontrol";
            //cph1_ucUnvan_cv.validateemptytext = "true";
            oSB.AppendLine("</script>");

            return MvcHtmlString.Create(oSB.ToString());
        }
    }
}