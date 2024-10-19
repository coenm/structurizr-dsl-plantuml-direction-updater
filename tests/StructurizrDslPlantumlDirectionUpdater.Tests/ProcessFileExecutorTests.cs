namespace StructurizrDslPlantumlDirectionUpdater.Tests;

using FluentAssertions;
using Xunit;

public class ProcessFileExecutorTests
{
    private readonly ProcessFileExecutor _sut = new();

    [Fact]
    public void ProcessContent_ShouldDoNothing_WhenInputDoesNotContainMarkers()
    {
        // arrange
        var input = "dummy";

        // act
        var result = _sut.ProcessContent(input);

        // assert
        result.Should().Be(input);
    }
    
    [Fact]
    public void ProcessContent_ShouldUpdate()
    {
        // arrange
        var input = "AINet.BlobService .[#707070,thickness=2].> AzureBlobstorage : \"<color:#707070>Calls\\n<color:#707070><size:8>[REST[puml length:1][puml down]]</size>\"";

        // act
        var result = _sut.ProcessContent(input);

        // assert
        result.Should().Be("""
                           AINet.BlobService .[#707070,thickness=2]d..> AzureBlobstorage : "<color:#707070>Calls\n<color:#707070><size:8>[REST]</size>"
                           """);
    }
}