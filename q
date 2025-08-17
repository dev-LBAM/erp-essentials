[33mcommit 740723d8ed20b8dd9c5505566f9423fc525bdb32[m[33m ([m[1;36mHEAD[m[33m -> [m[1;32mmain[m[33m, [m[1;31morigin/main[m[33m)[m
Author: dev-LBAM <lucasbatist.a@hotmail.com>
Date:   Thu Aug 14 21:35:54 2025 -0300

    test(application): Add unit tests for CreateProductHandler

[33mcommit 5067de23a8dab0c83a365344f81a8d37802fb9e8[m
Author: dev-LBAM <lucasbatist.a@hotmail.com>
Date:   Thu Aug 14 18:18:37 2025 -0300

    chore(domain): Standardize Catalogs folder naming to plural

[33mcommit c64dca02506bcc69edd1a092bcc98bee2c861b8a[m
Author: dev-LBAM <lucasbatist.a@hotmail.com>
Date:   Thu Aug 14 17:19:05 2025 -0300

    style(dotnet): Apply modern C# syntax and code analyzer suggestions

[33mcommit d787ecb536ffd06b120e30a1f57e0575311dee30[m
Author: dev-LBAM <lucasbatist.a@hotmail.com>
Date:   Thu Aug 14 17:17:53 2025 -0300

    feat(application): Implement CreateProduct feature
    
    -Adds CreateProductCommand, Handler, and Validator to manage the product creation use case following CQRS principles
    
    -Implements the full CancellationToken pattern, passing the token from the handler down to the repository interface for robust, cancellable async operations

[33mcommit 5e7a3e9011844e36a631ddbb8c752c9321949153[m
Author: dev-LBAM <lucasbatist.a@hotmail.com>
Date:   Wed Aug 13 17:30:22 2025 -0300

    test(domain): Add unit tests for Product aggregate

[33mcommit 9be7202ccd375b36cd7a441981fb6c7a6e34d10e[m
Author: dev-LBAM <lucasbatist.a@hotmail.com>
Date:   Tue Aug 12 17:46:44 2025 -0300

    test(domain): Add unit tests for Brand And Category aggregates

[33mcommit 014071472610f664e4d1537f8b7ac0b5698b6509[m
Author: dev-LBAM <lucasbatist.a@hotmail.com>
Date:   Tue Aug 12 16:29:05 2025 -0300

    refactor(domain): Apply Parameter Objects and explicit types
    
    Refactors the domain entities to improve clarity, safety, and maintainability, aligning with a more explicit coding style
    
    -The Introduce Parameter Object pattern was applied to methods with multiple parameters (Product.Create, Lot.Create, Product.ReceiveStock). This simplifies method signatures and makes them easier to extend
    
    -All uses of the 'var' keyword have been replaced with explicit type declarations to improve code readability and remove ambiguity

[33mcommit f8a7b9fecfad587e2be2bdd015c5a62eabc6d31c[m
Author: dev-LBAM <lucasbatist.a@hotmail.com>
Date:   Mon Aug 11 18:56:48 2025 -0300

    feat(domain): Define and implement the Product aggregate
    
    -Establishes the Product aggregate root, responsible for managing its state and the Lot child entities
    
    Includes rich business logic for stock management (AddLot, RemoveStock, GetTotalStock, AddQuantityToLot, RemoveQuantityFromLot)
    
    Define IProductRepository contract for persistence
    
    Implement a method Create that accumulates all validation errors

[33mcommit d4526fc48ffc19c1538a2cc620d531621bec98b8[m
Author: dev-LBAM <lucasbatist.a@hotmail.com>
Date:   Mon Aug 11 12:38:20 2025 -0300

    feat(domain): Define Brand and Category aggregates
    
    -Establishes Brand and Category entities with self-validating factory methods that include data standardization.
    
    -Defines repository interfaces and specific domain errors for the Catalog context

[33mcommit 7fc7f57076e56f69746de1f5db77485eb3477e09[m
Author: dev-LBAM <lucasbatist.a@hotmail.com>
Date:   Mon Aug 11 11:41:28 2025 -0300

    feat(sharedkernel): Add string extension for name standardization
    
    -Trims whitespace and applies title casing, preventing data inconsistencies and improving overall data quality before validation

[33mcommit da8d0b078facba1b35c540cf2cae4388b9b14c9d[m
Author: dev-LBAM <lucasbatist.a@hotmail.com>
Date:   Mon Aug 11 10:34:07 2025 -0300

    feat(sharedkernel): Implement a Result Pattern for erro handling
    
    -Established a robust exception free error handling strategy for the entire solution

[33mcommit 1dc1f0b7d01fdb7abaaaeb8db6b30e568abb5502[m
Author: dev-LBAM <lucasbatist.a@hotmail.com>
Date:   Sun Aug 10 17:21:14 2025 -0300

    chore(build): Initial project structure and solution setup
