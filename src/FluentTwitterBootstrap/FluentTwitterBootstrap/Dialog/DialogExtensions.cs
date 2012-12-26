using System;

namespace FluentTwitterBootstrap.Dialog
{
    public static class DialogExtensions
    {
        public static IDialogBuilder YesNo(this IDialogBuilder dialogBuilder, Func<IButtonBuilder, IButtonBuilder> yesConfiguration)
        {
            return dialogBuilder.WithButtons(y => yesConfiguration(y.Saying("Yes")), no => no.Saying("No").ClosesDialog);
        }
    }
}