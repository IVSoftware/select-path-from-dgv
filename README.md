Your post has several related questions:
- How to avoid error `The system cannot find the file specified`
- Is there a way to make each row a control or something?
- How to make a button in DataGrid control
- How to make a button open a file path on the same row

There might be a simple solution to avoiding the exception. Try this syntax for opening the file:

    System.Diagnostics.Process.Start("explorer.exe", path);

Since it's hard to say exactly where the problem might be, I will also offer a comprehensive answer to your questions.

[![enter image description here][1]][1]

***
**Locating files**

If the files are part of the install (known at compile time), they can be reliably located by setting the "Copy to Output Directory" property and referencing them as 

    path =
        Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Images",
            "boardgamePack_v2",
            "PNG",
            "Cards",
            "shortFileName.png" // Example filename
        );

[![enter image description here][2]][2]

        
On the other hand, if they can be modified by the user, they belong in 

    path =        
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "AppName", // Example app name
            "Images",
            "boardgamePack_v2",
            "PNG",
            "Cards",
            "shortFileName.png" // Example filename
        ));

***
**Define row behavior**

In order to _"make each row a [...] something"_ requires a class that has public properties corresponding to the columns in the DataGridView. This class will be wound to the `DataSource` property of the DataGridView, for example by making a `BindingList<Card>` Here's a minimal example:

    class Card
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public Image Image{ get; set; }

        internal static string ImageBaseFolder { get; set; } = string.Empty;
        public string GetFullPath() => Path.Combine(ImageBaseFolder, FilePath);
    }

***
**Making a button in grid control**

In this sample a DataGridView control gets initialized in the method that loads the MainForm. Since there is no property of the class corresponding to the Open column, we will have to add it after the columns have been auto-generated from the binding. A handler is added that we can inspect to determine whether a button has been clicked.

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Card.ImageBaseFolder = 
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Images",
                    "boardgamePack_v2",
                    "PNG",
                    "Cards"
                );
        }
        internal BindingList<Card> Cards { get; } = new BindingList<Card>();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dataGridViewCards.DataSource = Cards;

            #region F O R M A T    C O L U M N S
            Cards.Add(new Card()); // <- Auto generate columns
            dataGridViewCards.Columns["Name"].AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCards.Columns["FilePath"].AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill;
            DataGridViewImageColumn imageColumn = (DataGridViewImageColumn) dataGridViewCards.Columns["Image"];
            imageColumn.AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill;
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.Width = 100;
            imageColumn.HeaderText = string.Empty;

            // Add the button column (which is not auto-generated).
            dataGridViewCards.Columns.Insert(2, new DataGridViewButtonColumn
            {
                Name = "Open",
                HeaderText = "Open",
            });
            Cards.Clear();
            #endregion F O R M A T    C O L U M N S

            // Add a few cards
            Cards.Add(new Card(Value.Ten, Suit.Diamonds));
            Cards.Add(new Card(Value.Jack, Suit.Clubs));
            Cards.Add(new Card(Value.Queen, Suit.Diamonds));
            Cards.Add(new Card(Value.King, Suit.Clubs));
            Cards.Add(new Card(Value.Ace, Suit.Hearts));

            dataGridViewCards.ClearSelection();

            // Detect click on button or any other cell.
            dataGridViewCards.CellContentClick += onAnyCellContentClick;
        }


**Make a button open a file path on the same row**

        In the handler, retrieve the card from the bound collection by index and get the full path. You may have better luck with the `Process.Start` if you use the syntax shown:

        private void onAnyCellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCards.Columns[e.ColumnIndex].Name.Equals("Open"))
            {
                var card = Cards[e.RowIndex];
                var path = card.GetFullPath();
                Process.Start("explorer.exe", path);
            }
        }
    }


  [1]: https://i.stack.imgur.com/9iBv0.png
  [2]: https://i.stack.imgur.com/EjQ3l.png