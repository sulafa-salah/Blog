using Blog.Domain.Models;


namespace Blog.Test
{
    public class CategoryTest
    {
        [Fact]
        public void  CategoryConstructor_ConstructCatogory_IsDeteleMustBeFalse() {
            //Arrange => setting up the test 
            //nothing to see here 

            //Act => exceuting  the actual test 
            var catogry = new Category();
            catogry.Name = "Test";

            // Assert => verifing the executed action 
            Assert.False(catogry.IsDeleted);

        }
       
        
    }
}
