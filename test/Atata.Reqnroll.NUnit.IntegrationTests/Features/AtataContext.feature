Feature: AtataContext

Scenario: Scenario AtataContext is AtataContext.Current
	Then Scenario AtataContext is AtataContext.Current

Scenario: Scenario parent AtataContext is a feature context
	Then Scenario parent AtataContext is a feature context

Scenario: Scenario grandparent AtataContext is a global context
    Then Scenario grandparent AtataContext is a global context
