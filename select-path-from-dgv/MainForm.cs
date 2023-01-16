using System.ComponentModel;

namespace select_picture_from_dgv
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();
        internal BindingList<Card> Cards { get; } = new BindingList<Card>();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dataGridViewCards.AllowUserToAddRows = false;
            dataGridViewCards.RowHeadersVisible = false;
            dataGridViewCards.DataSource = Cards;
            dataGridViewCards.RowTemplate.Height = 100;
            #region F O R M A T    C O L U M N S
            Cards.Add(new Card()); // <- Auto generate columns
            dataGridViewCards.Columns["Name"].AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCards.Columns["FilePath"].AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill;
            DataGridViewImageColumn imageColumn = (DataGridViewImageColumn) dataGridViewCards.Columns["Image"];
            imageColumn.AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill;
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageColumn.Width = 50;
            imageColumn.HeaderText = string.Empty;
            dataGridViewCards.Columns.Insert(2, new DataGridViewButtonColumn
            {
                HeaderText = "Open",
            });
            //dataGridViewCards.Columns.Add(new DataGridViewImageColumn
            //{
            //    HeaderText = "Image",
            //});
            Cards.Clear();
            #endregion F O R M A T    C O L U M N S

            // Add a few cards
            Cards.Add(new Card { Value = Value.Ten, Suit = Suit.Diamonds });
            Cards.Add(new Card { Value = Value.Jack, Suit = Suit.Clubs });
            Cards.Add(new Card { Value = Value.Queen, Suit = Suit.Hearts });
            Cards.Add(new Card { Value = Value.King, Suit = Suit.Spades });
            Cards.Add(new Card { Value = Value.Ace, Suit = Suit.Diamonds });

            dataGridViewCards.ClearSelection();
            //dataGridViewCards.SelectionChanged += onSelectionChanged;
            //dataGridViewCards.CurrentCellChanged += onSelectionChanged;
            //pictureBoxCard.Click += (sender, e) => dataGridViewCards.CurrentCell = null;
        }
        //private void onSelectionChanged(object? sender, EventArgs e)
        //{
        //    if (dataGridViewCards.CurrentCell == null)
        //    {
        //        pictureBoxCard.Image = getCardImage(Value.Back);
        //    }
        //    else
        //    {
        //        int row = dataGridViewCards.CurrentCell.RowIndex;
        //        if((row != -1) && (row < Cards.Count)) 
        //        {
        //            Card card = Cards[row];
        //            pictureBoxCard.Image = getCardImage(card);
        //        }
        //    }
        //}
    }
    enum Value { Joker, Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Back }
    enum Suit { Clubs, Diamonds, Hearts, Spades, }
    class Card
    {
        public Card() => Image = getCardImage(Value.Back);
        public string Name => $"{Value} of {Suit}";
        public string FilePath
        {
            get
            {
                switch (Value)
                {
                    case Value.Ace:
                        return $"card{Suit}A.png";
                    case Value.Jack:
                        return $"card{Suit}J.png";
                    case Value.Queen:
                        return $"card{Suit}Q.png";
                    case Value.King:
                        return $"card{Suit}K.png";
                    case Value.Joker:
                        return $"cardJoker.png";
                    case Value.Back:
                        return $"cardBack_green3.png";
                    default:
                        return $"card{Suit}{(int)Value}.png";
                }
            }
        }

        [Browsable(false)]
        public Value Value { get; internal set; }

        [Browsable(false)]
        public Suit Suit { get; internal set; }

        public Image Image {get; set; }
        public Image getCardImage(Value value = Value.Joker, Suit? suit = null)
        {
            string _imageBase = $"{GetType().Namespace}.Images.boardgamePack_v2.PNG.Cards";
            switch (value)
            {
                case Value.Ace:
                    return localImageFromResourceName($"{_imageBase}.card{suit}A.png");
                case Value.Jack:
                    return localImageFromResourceName($"{_imageBase}.card{suit}J.png");
                case Value.Queen:
                    return localImageFromResourceName($"{_imageBase}.card{suit}Q.png");
                case Value.King:
                    return localImageFromResourceName($"{_imageBase}.card{suit}K.png");
                case Value.Joker:
                    return localImageFromResourceName($"{_imageBase}.cardJoker.png");
                case Value.Back:
                    return localImageFromResourceName($"{_imageBase}.cardBack_green3.png");
                default:
                    return localImageFromResourceName($"{_imageBase}.card{suit}{(int)value}.png");
            }
            Image localImageFromResourceName(string resource)
            {
                using (var stream = GetType().Assembly.GetManifestResourceStream(resource)!)
                {
                    return new Bitmap(stream);
                }
            }
        }
    }
}