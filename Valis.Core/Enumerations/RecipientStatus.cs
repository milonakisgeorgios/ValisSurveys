namespace Valis.Core
{
    public enum RecipientStatus : byte
    {
        None = 0,
        /// <summary>
        /// Ο αποδέκτης άνοιξε το survey
        /// </summary>
        OpenSurvey = 1,
        /// <summary>
        /// O αποδέκτης έχει απαντήσει σε τουλάχιστον μία ερώτηση απο το ερωτηματολόγιο
        /// </summary>
        PartiallyCompleted = 2,
        /// <summary>
        /// Ο αποδέκτης τερμάτισε όλο το ερωτηματολόγιο
        /// </summary>
        Completed = 3,
        /// <summary>
        /// Ο αποδέκτης δεν πληρούσε τα κριτήρια για να συμμετάσχει στην έρευνα
        /// </summary>
        Disqualified = 4
    }
}
