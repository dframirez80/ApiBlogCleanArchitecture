using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Constants
{
    // Esta clase tiene problema de cohesión. Existen niveles de cohesión que te invito a investigar.
    // Ej: https://www.google.com/search?q=cohesion+programacion
    public static class Numerical
    {
        public enum StatusUser { Active, Blocked, Pending }; // Esto debería estar cerca de User.
        public const int MinValue = 0; // Int.MinValue, ya existe. Podría llamarse PositiveIntMinValue o algo similar.
        public const int MaxValueReaction = 1; // Estas tiene más relacion con Reaction que con el solo hecho de ser numéricas
        public const int MinValueReaction = 0; // Estas tiene más relacion con Reaction que con el solo hecho de ser numéricas
    }
}
