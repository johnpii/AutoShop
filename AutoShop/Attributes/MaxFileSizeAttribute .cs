using System.ComponentModel.DataAnnotations;

namespace AutoShop.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSizeInBytes;

        public MaxFileSizeAttribute(int maxFileSizeInBytes)
        {
            _maxFileSizeInBytes = maxFileSizeInBytes;
            ErrorMessage = "Картинка не должна быть больше " + _maxFileSizeInBytes + " байт";
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (value is IFormFile file)
                {
                    if (file.Length < _maxFileSizeInBytes)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
