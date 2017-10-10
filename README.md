#Formula Parser

Components implemented in this repository are designed in order to address the requierements to make calculation and transform data in form of timeseries (as on example). Time series data records should be able to be calculated based on other records, on formulas provided both within series or from other sources in the following form
  
```bash
PCH(LOG{1, ANNUAL}, '2010-01-01')
```

Time series data sample

| Coutnry       | Concept       | Currency  | Scale        | Id     | 2010       | 2011       | 2012       |
| ------------- |:-------------:| ---------:|-------------:|-------:|-----------:|-----------:|-----------:|
| USA           | GDP           | USD       | Billions     | USAGDP | $12,532.32 | $12,567.45 | $13,325.53 |
| Canada        | GDP           | CAD       | Millions     | CAGDP  | $7,3212.22 | $7,589.55  | 8,021.31   |


##Components are separated to the following abstractions: 
  1. Antlr - The definition of formulas lexis and .net classes generation based on Antlr model
  2. Expressions - definition of generic expressions, .net expressions are not used as too redundant
  3. FormulaExpressions - generic components that translate formula equations to expressions tree
  4. Data - definition classes to represent, materialize and format time series data
  5. Data.Calculation - application of formulas to meet requirements of transformation timeseries data

##Sample appplication is implemented in Client console app with the following steps. 
  1. Loads time series data from csv file
  2. Applies hardcored series transformation to each time series example (//TODO: describe)
  3. Generates formatted output