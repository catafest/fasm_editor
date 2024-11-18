using Gtk;
using System;
using System.IO;

class Editor : TextView {
    public Editor() {
        WrapMode = WrapMode.Word;
        Editable = true;
    }
}

class FasmEditor : Window {
    private TextView textEditor;
    private string currentFile = null;

    public FasmEditor() : base("Fasm editor - by mythcat")
    {
        SetDefaultSize(640, 480);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        // Create the text editor first
        textEditor = new TextView();
        textEditor.WrapMode = WrapMode.Word;
        textEditor.Editable = true;

        // Create scrolled window for text editor
        ScrolledWindow scrolledWindow = new ScrolledWindow();
        scrolledWindow.Add(textEditor);

        // Create the main layout
        VBox vbox = new VBox(false, 2);
        
        // Create and add menubar
        MenuBar menubar = new MenuBar();
        AccelGroup agr = new AccelGroup();
        AddAccelGroup(agr);

        // File Menu
        Menu fileMenu = CreateFileMenu(agr);
        MenuItem file = new MenuItem("File");
        file.Submenu = fileMenu;

        // Run Menu
        Menu runMenu = CreateRunMenu(agr);
        MenuItem run = new MenuItem("Run");
        run.Submenu = runMenu;

        // Settings Menu
        Menu settingsMenu = CreateSettingsMenu(agr);
        MenuItem settings = new MenuItem("Settings");
        settings.Submenu = settingsMenu;

        menubar.Append(file);
        menubar.Append(run);
        menubar.Append(settings);

        // Add menubar and scrolled window to vbox
        vbox.PackStart(menubar, false, false, 0);
        vbox.PackStart(scrolledWindow, true, true, 0);

        Add(vbox);
        ShowAll();
    }

    private Menu CreateFileMenu(AccelGroup agr)
    {
        Menu fileMenu = new Menu();

        var newItem = new ImageMenuItem(Stock.New, agr);
        newItem.AddAccelerator("activate", agr, new AccelKey(
            Gdk.Key.n, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
        newItem.Activated += OnNewClicked;

        var openItem = new ImageMenuItem(Stock.Open, agr);
        openItem.AddAccelerator("activate", agr, new AccelKey(
            Gdk.Key.o, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
        openItem.Activated += OnOpenClicked;

        var saveItem = new ImageMenuItem(Stock.Save, agr);
        saveItem.AddAccelerator("activate", agr, new AccelKey(
            Gdk.Key.s, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
        saveItem.Activated += OnSaveClicked;

        var exitItem = new ImageMenuItem(Stock.Quit, agr);
        exitItem.AddAccelerator("activate", agr, new AccelKey(
            Gdk.Key.q, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
        exitItem.Activated += OnQuitClicked;

        fileMenu.Append(newItem);
        fileMenu.Append(openItem);
        fileMenu.Append(saveItem);
        fileMenu.Append(new SeparatorMenuItem());
        fileMenu.Append(exitItem);

        return fileMenu;
    }

    private Menu CreateRunMenu(AccelGroup agr)
    {
        Menu runMenu = new Menu();

        var executeItem = new ImageMenuItem(Stock.Execute, agr);
        executeItem.Label = "Execute";
        executeItem.AddAccelerator("activate", agr, new AccelKey(
            Gdk.Key.F9, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
        executeItem.Activated += OnExecuteClicked;

        var compileItem = new ImageMenuItem(Stock.Convert, agr);
        compileItem.Label = "Compile";
        compileItem.AddAccelerator("activate", agr, new AccelKey(
            Gdk.Key.F5, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
        compileItem.Activated += OnCompileClicked;

        runMenu.Append(executeItem);
        runMenu.Append(compileItem);

        return runMenu;
    }

    private Menu CreateSettingsMenu(AccelGroup agr)
    {
        Menu settingsMenu = new Menu();

        var settingsItem = new ImageMenuItem(Stock.Preferences, agr);
        settingsItem.Label = "Settings";
        settingsItem.AddAccelerator("activate", agr, new AccelKey(
            Gdk.Key.F3, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
        settingsItem.Activated += OnSettingsClicked;

        settingsMenu.Append(settingsItem);

        return settingsMenu;
    }

    private void OnNewClicked(object sender, EventArgs args)
    {
        textEditor.Buffer.Text = "";
        currentFile = null;
    }

    private void OnOpenClicked(object sender, EventArgs args)
    {
        FileChooserDialog fc = new FileChooserDialog("Open File", this,
            FileChooserAction.Open,
            "Cancel", ResponseType.Cancel,
            "Open", ResponseType.Accept);

        if (fc.Run() == (int)ResponseType.Accept)
        {
            try
            {
                string content = File.ReadAllText(fc.Filename);
                textEditor.Buffer.Text = content;
                currentFile = fc.Filename;
            }
            catch (Exception ex)
            {
                ShowError("Error opening file: " + ex.Message);
            }
        }
        fc.Destroy();
    }

    private void OnSaveClicked(object sender, EventArgs args)
    {
        FileChooserDialog fc = new FileChooserDialog("Save File", this,
            FileChooserAction.Save,
            "Cancel", ResponseType.Cancel,
            "Save", ResponseType.Accept);

        fc.DoOverwriteConfirmation = true;

        if (currentFile != null)
        {
            fc.SetFilename(currentFile);
        }

        if (fc.Run() == (int)ResponseType.Accept)
        {
            try
            {
                TextIter start, end;
                textEditor.Buffer.GetBounds(out start, out end);
                string content = textEditor.Buffer.GetText(start, end, true);
                File.WriteAllText(fc.Filename, content);
                currentFile = fc.Filename;
            }
            catch (Exception ex)
            {
                ShowError("Error saving file: " + ex.Message);
            }
        }
        fc.Destroy();
    }

    private void OnExecuteClicked(object sender, EventArgs args)
    {
        // TODO: Implement execute functionality
    }

    private void OnCompileClicked(object sender, EventArgs args)
    {
        // TODO: Implement compile functionality
    }

    private void OnSettingsClicked(object sender, EventArgs args)
    {
        // TODO: Implement settings dialog
    }

    private void OnQuitClicked(object sender, EventArgs args)
    {
        Application.Quit();
    }

    private void ShowError(string message)
    {
        MessageDialog md = new MessageDialog(this,
            DialogFlags.DestroyWithParent, MessageType.Error,
            ButtonsType.Close, message);
        md.Run();
        md.Destroy();
    }

    public static void Main()
    {
        Application.Init();
        new FasmEditor();
        Application.Run();
    }
}
