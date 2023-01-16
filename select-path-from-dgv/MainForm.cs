using System.ComponentModel;

namespace select_picture_from_dgv
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Card.ImageBase = 
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
            imageColumn.Width = 100;
            imageColumn.HeaderText = string.Empty;
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
            dataGridViewCards.CellContentClick += onAnyCellContentClick;
        }

        private void onAnyCellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCards.Columns[e.ColumnIndex].Name.Equals("Open"))
            {
                var card = Cards[e.RowIndex];
                card.Image = card.GetCardImage();
            }
        }
    }
    enum Value { Joker, Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Back }
    enum Suit { Clubs, Diamonds, Hearts, Spades, }
    class Card
    {
        public Card(Value value = Value.Back, Suit? suit = null)
        {
            Value = value;
            Suit = suit ?? (Suit)-1;
            Image = GetCardImage();
        }
        internal static string ImageBase { get; set; }
        public string Name => $"{Value} of {Suit}";
        public string FilePath
        {
            get
            {
                switch (Value)
                {
                    case Value.Ace: return $"card{Suit}A.png";
                    case Value.Jack: return $"card{Suit}J.png";
                    case Value.Queen: return $"card{Suit}Q.png";
                    case Value.King: return $"card{Suit}K.png";
                    case Value.Joker: return $"cardJoker.png";
                    case Value.Back: return $"cardBack_green3.png";
                    default: return $"card{Suit}{(int)Value}.png";
                }
            }
        }

        [Browsable(false)]
        public Value Value { get; internal set; }

        [Browsable(false)]
        public Suit Suit { get; internal set; }
        public Image Image { get; set; }

        public Image GetCardImage(bool faceUp = true)
        {
            if (faceUp)
            {
                switch (Value)
                {
                    case Value.Ace: return localToImage($"card{Suit}A.png");
                    case Value.Jack: return localToImage($"card{Suit}J.png");
                    case Value.Queen: return localToImage($"card{Suit}Q.png");
                    case Value.King: return localToImage($"card{Suit}K.png");
                    case Value.Joker: return localToImage($"cardJoker.png");
                    case Value.Back:
                        return localToImage($"cardBack_green3.png");
                    default:
                        return localToImage($"card{Suit}{(int)Value}.png");
                }
            }
            else
            {
                return localToImage($"cardBack_green3.png");
            }
            Image localToImage(string shortFileName) => 
                Image.FromFile(Path.Combine(ImageBase, shortFileName));
        }
    }
}