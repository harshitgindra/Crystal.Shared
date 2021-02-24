<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** reference: https://github.com/harshitgindra/Crystal.Shared/edit/master/README.md
-->


| | |
|-|-|
| Contributors | [![Contributors][contributors-shield]][contributors-url]
| Forks | [![Forks][forks-shield]][forks-url]
| Issues | [![Issues][issues-shield]][issues-url]
| MIT License | [![MIT License][license-shield]][license-url]
| LinkedIn | [![LinkedIn][linkedin-shield]][linkedin-url]
| Nuget | [![Nuget](https://img.shields.io/nuget/v/Crystal.EntityFrameworkCore)](https://www.nuget.org/packages/Crystal.EntityFrameworkCore/)
| Stargazers | [![Stargazers][stars-shield]][stars-url]
| Github Actions | [![Actions](https://github.com/harshitgindra/Crystal.Shared/workflows/Main%20workflow/badge.svg)](https://github.com/harshitgindra/Crystal.Shared/actions?query=workflow%3A%22Main+workflow%22) |



<!-- TABLE OF CONTENTS -->
## Table of Contents

* [About the Project](#about-the-project)
  * [Built With](#built-with)
* [Getting Started](#getting-started)
* [Usage](#usage)
* [Roadmap](#roadmap)
* [Contributing](#contributing)
* [License](#license)
* [Contact](#contact)
* [Acknowledgements](#acknowledgements)



<!-- ABOUT THE PROJECT -->
## About The Project
This project contains some of the shared components that can be reused in any ASP.NET applications(Web, Api, Desktop and Mobile). One of the reason of builidng this library is to standardize the repository implementation. Whenever we build a repository and query data, it is important to make sure it is disposed after it is used. Sometimes we do it, sometimes we forget. It could be one of the biggest factor for failure of several applications as the scalability of the app is affected. The `Unit Of Work` implementation in this project makes sure that the connection to the database is disposed and we do not leave any room for memory leaks or open contexts


### Built With
The libraries used in the project includes
* [Entity Framework Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)
* [Dynamic Linq](https://www.nuget.org/packages/System.Linq.Dynamic.Core/)
* [Newtonsoft Json](https://www.nuget.org/packages/Newtonsoft.Json/)
* [Xamarin Forms](https://www.nuget.org/packages/Xamarin.Forms/5.0.0.1558-pre3)
* [Xamarin Essentials](https://www.nuget.org/packages/Xamarin.Essentials/1.6.0-pre2)
* [Z.EntityFramework.Extensions.EFCore](https://www.nuget.org/packages/Z.EntityFramework.Extensions.EFCore/5.0.0-rc.2.20475.6-02)
* [NUnit](https://www.nuget.org/packages/NUnit/)


<!-- GETTING STARTED -->
## Getting Started

In this library, there are some shared code that you can reuse.


### Unit of work with Entity Framework Core
This libary provides with a sharable code to implement Unit of Work pattern within your repository. 

Start with downloading the the following nuget packages
* [Crystal.Patterns.Abstraction](https://www.nuget.org/packages/Crystal.Patterns.Abstraction/)
* [Crystal.EntityFrameworkCore](https://www.nuget.org/packages/Crystal.EntityFrameworkCore/)
* [Crystal.Shared](https://www.nuget.org/packages/Crystal.Shared/)

Lets start with creating a `DBContext` class. Make sure it extends `BaseContext`.

```
    public class TestContext : BaseContext
    {
        public DbSet<Order> Orders { get; set; }

        public TestContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase($"TestDb");

            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
    
```

Depending upon what database provider you plan to use, you can switch the the configuration from InMemory, Sqlite and Sql Server.

Create an new interface `IUowRepository` that implements `IBaseUowRepository`

```
    public interface IUowRepository : IBaseUowRepository
    {
    }

```

Now, let's build the implementation class for *unit of work*

```
    public class UowRepository : BaseUowRepository, IUowRepository
    {
        public UowRepository() : base(new TestContext())
        {

        }

        public TestContext Context => (TestContext)this.DbContext;
    }
    
```

Basis of Unit of Work Repository is to share the database context across multiple entites when accessing database and having a common or generic implementation for all CRUD operations.

```
        private IBaseRepository<Order> _orderRepository;

        public IBaseRepository<Order> Orders
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new BaseRepository<Order>(this.DbContext);
                }
                return _orderRepository;
            }
        }
        
```

Just so we can access the Order Repository instance, create a getter in `IUowRepository`

```

    public interface IUowRepository : IBaseUowRepository
    {
        IBaseRepository<Order> Orders { get;}
    }
    
```

You can register `IUowRepository` and `UowRepository` or create a new instance


## Usage

```
    IUowRepository uow = new UowRepository();
    var records = await uow.Orders.GetAsync();
            
```
            
There are different methods available for different operations. Check out `BaseRepository` class for more details

You can also configure it with jQuery datatables implementation. All the basic serverside implementation can be performed using this extension method `ToDatatable()`.

```

                   [HttpPost]
        public async Task<JsonResult> GetAll(DataTableRequest<Entity.Information> request)
        {
            var result = (await _unitOfWorkRepository.Informations.QueryAsync())
                              .ToDatatable(request);

            return Json(result);
        }
            
```

 You can refer to the test project which contains few examples on how to use the library for different CRUD operations.           
            


<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/harshitgindra/Crystal.Shared/issues) for a list of proposed features (and known issues).


<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.


<!-- CONTACT -->
## Contact

Your Name - [@harshitgindra](https://twitter.com/harshitgindra) - harshitgindra@gmail.com

Project Link: [https://github.com/harshitgindra/Crystal.Shared](https://github.com/harshitgindra/Crystal.Shared)

<!-- ACKNOWLEDGEMENTS -->
## Acknowledgements
* [Img Shields](https://shields.io)
* [Choose an Open Source License](https://choosealicense.com)
* [Github Readme template](https://github.com/othneildrew/Best-README-Template)




<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[contributors-shield]: https://img.shields.io/github/contributors/harshitgindra/Crystal.Shared.svg?style=flat-square
[contributors-url]: https://github.com/harshitgindra/Crystal.Shared/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/harshitgindra/Crystal.Shared.svg?style=flat-square
[forks-url]: https://github.com/harshitgindra/Crystal.Shared/network/members
[stars-shield]: https://img.shields.io/github/stars/harshitgindra/Crystal.Shared.svg?style=flat-square
[stars-url]: https://github.com/harshitgindra/Crystal.Shared/stargazers
[issues-shield]: https://img.shields.io/github/issues/harshitgindra/Crystal.Shared.svg?style=flat-square
[issues-url]: https://github.com/harshitgindra/Crystal.Shared/issues
[license-shield]: https://img.shields.io/github/license/harshitgindra/Crystal.Shared.svg?style=flat-square
[license-url]: https://github.com/harshitgindra/Crystal.Shared/blob/master/LICENSE.txt
[license-shield]: https://img.shields.io/github/license/harshitgindra/Crystal.Shared.svg?style=flat-square
[license-url]: https://github.com/harshitgindra/Crystal.Shared/blob/master/LICENSE.txt

[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=flat-square&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/harshit-gindra
