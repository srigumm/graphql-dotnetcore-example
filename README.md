### Add package

          dotnet add package HotChocolate
    dotnet add package HotChocolate.AspNetCore
    dotnet add package HotChocolate.AspNetCore.Playground
    (or)
    <PackageReference Include="HotChocolate.AspNetCore" Version="0.8.2" />
    <PackageReference Include="HotChocolate.AspNetCore.GraphiQL" Version="0.8.2" />
    <PackageReference Include="HotChocolate.AspNetCore.Playground" Version="0.8.2" />
    <PackageReference Include="HotChocolate.AspNetCore.Authorization" Version="0.8.2" />
    <PackageReference Include="HotChocolate.Subscriptions.InMemory" Version="0.8.2" />

### Define Schema file. (schema.graphql)

          type Query {
            cards: [CreditCard!]!
            card(Id: ID!): CreditCard
          }

          type CreditCard {
            id: ID!
            name: String!
            IssuedBy: String
          }

### Configure middleware

          using HotChocolate;
          using HotChocolate.AspNetCore;
          using HotChocolate.Execution.Configuration;

           // this enables you to use DataLoader in your resolvers.
            services.AddDataLoaderRegistry();

            // Add GraphQL Services
            var queryExecutionOptions = new QueryExecutionOptions
            {
                TracingPreference = TracingPreference.OnDemand
            };

            if (_environment.IsDevelopment())
            {
                queryExecutionOptions.TracingPreference = TracingPreference.Always;
                queryExecutionOptions.IncludeExceptionDetails = true;
                _logger.LogInformation(" *** LogInformation: In DEV Environment");

            }
          // Create our schema
          var schemaDef = File.ReadAllText("schema.graphql");
          var schema = Schema.Create(schemaDef, ConfigureSchema);

          services.AddGraphQL(schema, queryExecutionOptions);



          app.UseGraphQL();
          app.UsePlayground();

### Launch playground and test queries

      {
        cards{
          id
          name
        }
        card(Id:2){
          id
          name
        }
      }
