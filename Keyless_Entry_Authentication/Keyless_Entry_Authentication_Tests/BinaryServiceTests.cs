using Moq;
using Xunit;
using Keyless_Entry_Authentication.Services;

namespace Keyless_Entry_Authentication.Tests
{
    public class BinaryServiceTests
    {
        [Fact]
        public void ByteGeneratorShouldReturnByteArray()
        {
            // Arrange
            var byteLength = 5;
            var binaryService = new Mock<BinaryService>();

            // Act
            var result = binaryService.Object.ByteGenerator();

            // Assert
            Assert.IsAssignableFrom<byte[]>(result);
            Assert.Equal(result.Length, byteLength);
        }

        [Fact]
        public void BinaryRepresentationShouldReturnString()
        {
            // Arrange
            var binLength = 50;
            var binaryService = new Mock<BinaryService>();

            // Act
            var byteList = binaryService.Object.ByteGenerator();
            var result = binaryService.Object.BinaryRepresentation(byteList);

            //Assert
            Assert.IsAssignableFrom<string>(result);
            Assert.Equal(result.Length, binLength);
        }
    }
}
