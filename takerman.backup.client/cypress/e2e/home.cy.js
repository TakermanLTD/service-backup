/// <reference types="cypress" />

describe('home', () => {
  it('successfully loads', () => {
    cy.visit('/');
    cy.url().should('include', '/');
  });

  it("has elements on the page", () => {
    cy.visit('/');
    cy.get('header').should('exist');
    cy.get('footer').should('exist');
  });
});
