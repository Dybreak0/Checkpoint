using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text.Method;
using Android.Views;
using Android.Views.InputMethods;
using MobileJO.Droid.CustomRenderers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(CustomEditorRenderer))]
namespace MobileJO.Droid.CustomRenderers
{
    internal class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer(Android.Content.Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var nativeEditText = (global::Android.Widget.EditText)Control;

                //While scrolling inside Editor stop scrolling parent view.
                nativeEditText.OverScrollMode = OverScrollMode.Always;
                nativeEditText.ScrollBarStyle = ScrollbarStyles.InsideInset;
                nativeEditText.SetOnTouchListener(new DroidTouchListener());

                //For Scrolling in Editor innner area
                Control.VerticalScrollBarEnabled = true;
                Control.MovementMethod = ScrollingMovementMethod.Instance;
                Control.ScrollBarStyle = ScrollbarStyles.InsideInset;
                //Force scrollbars to be displayed
                Android.Content.Res.TypedArray a = Control.Context.Theme.ObtainStyledAttributes(new int[0]);
                InitializeScrollbars(a);
                a.Recycle();
            }

            if (Control == null)
                return;

            Control.Background = null;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Control.SetTextIsSelectable(true);
                Control.CustomSelectionActionModeCallback = new CustomSelectionActionModeCallback();
                Control.CustomInsertionActionModeCallback = new CustomInsertionActionModeCallback();
            }
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            base.DispatchTouchEvent(e);

            var imm = (InputMethodManager)Control.Context.GetSystemService(Android.Content.Context.InputMethodService);
            imm.ShowSoftInput(Control, 0);

            return true;
        }

        private class CustomInsertionActionModeCallback : Java.Lang.Object, ActionMode.ICallback
        {
            private const int PasteId = Android.Resource.Id.Paste;
            private const int SelectAllId = Android.Resource.Id.SelectAll;

            public bool OnCreateActionMode(ActionMode mode, IMenu menu) => true;

            public bool OnActionItemClicked(ActionMode m, IMenuItem i) => false;

            public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
            {
                try
                {
                    var pasteItem = menu.FindItem(PasteId);
                    var selectAllItem = menu.FindItem(SelectAllId);

                    var paste = pasteItem.TitleFormatted;
                    var selectAll = selectAllItem.TitleFormatted;

                    menu.Clear();
                    menu.Add(0, SelectAllId, 0, selectAll);
                    menu.Add(0, PasteId, 0, paste);
                }
                catch
                {
                    // ignored
                }

                return true;
            }

            public void OnDestroyActionMode(ActionMode mode) { }
        }

        private class CustomSelectionActionModeCallback : Java.Lang.Object, ActionMode.ICallback
        {
            private const int CopyId = Android.Resource.Id.Copy;
            private const int CutId = Android.Resource.Id.Cut;

            public bool OnActionItemClicked(ActionMode m, IMenuItem i) => false;

            public bool OnCreateActionMode(ActionMode mode, IMenu menu) => true;

            public void OnDestroyActionMode(ActionMode mode) { }

            public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
            {
                try
                {
                    var copyItem = menu.FindItem(CopyId);
                    var cutItem = menu.FindItem(CutId);

                    var copy = copyItem.TitleFormatted;
                    var cut = cutItem.TitleFormatted;

                    menu.Clear();
                    menu.Add(0, CutId, 0, cut);
                    menu.Add(0, CopyId, 0, copy);

                }
                catch
                {
                    // ignored
                }

                return true;
            }
        }

    }
}