using Gtk;
using System;
class Editor : TextView {
	
}
class FasmEditor : Window {

	public FasmEditor() : base("Fasm editor - by mythcat")
	{
		SetDefaultSize(640, 480);
		SetPosition(WindowPosition.Center);
		DeleteEvent += delegate { Application.Quit(); };

		MenuBar my_menubar = new MenuBar();

		Menu filemenu = new Menu();
		MenuItem file = new MenuItem("File");
		file.Submenu = filemenu;

		Menu runmenu = new Menu();
		MenuItem run = new MenuItem("Run");
		run.Submenu = runmenu;
		AccelGroup agr = new AccelGroup();
		AddAccelGroup(agr);

		ImageMenuItem newi = new ImageMenuItem(Stock.New, agr);
		newi.AddAccelerator("activate", agr, new AccelKey(
			Gdk.Key.n, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
		filemenu.Append(newi);

		ImageMenuItem open = new ImageMenuItem(Stock.Open, agr);
		open.AddAccelerator("activate", agr, new AccelKey(
			Gdk.Key.n, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
		filemenu.Append(open);

		SeparatorMenuItem sep = new SeparatorMenuItem();
		filemenu.Append(sep);

		ImageMenuItem exit = new ImageMenuItem(Stock.Quit, agr);
		exit.AddAccelerator("activate", agr, new AccelKey(
			Gdk.Key.q, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
		// 
		ImageMenuItem runn = new ImageMenuItem(Stock.Execute, agr);
		newi.AddAccelerator("activate", agr, new AccelKey(
			Gdk.Key.F9, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
		runmenu.Append(runn);
		ImageMenuItem compile = new ImageMenuItem(Stock.Execute, agr);

		newi.AddAccelerator("activate", agr, new AccelKey(
			Gdk.Key.F5, Gdk.ModifierType.ControlMask, AccelFlags.Visible));
		runmenu.Append(compile);

		exit.Activated += OnActivated;
		filemenu.Append(exit);

		my_menubar.Append(file);
		my_menubar.Append(run);
		TextView textview = new TextView ();
		VBox vbox = new VBox(false, 2);
		vbox.PackStart(my_menubar, false, false, 0);
		//vbox.PackStart(new Label(), false, false, 0);
		vbox.PackStart(textview, false, false, 0);

		Add(vbox);

		ShowAll();
	}

	void OnActivated(object sender, EventArgs args)
	{
		Application.Quit();
	}


	public static void Main()
	{
		Application.Init();
		new FasmEditor();
		Application.Run();
	}
}