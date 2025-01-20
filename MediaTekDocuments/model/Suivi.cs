

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe suivi de commande
    /// </summary>
    public class Suivi
    {
        public string id { get; set; }

        public string etapeSuivi { get; set; }

        public Suivi (string id, string etapeSuivi)
        {
            this.id = id;
            this.etapeSuivi = etapeSuivi;
        }
    }
}
