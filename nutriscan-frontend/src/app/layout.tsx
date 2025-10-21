import type { Metadata } from 'next';
import './globals.css';
import { ReactNode } from 'react';
import Providers from './providers';

export const metadata: Metadata = {
  title: 'NutriScan',
  description: 'Escaneie alimentos, acompanhe sua nutrição e mantenha hábitos saudáveis.'
};

export default function RootLayout({ children }: { children: ReactNode }) {
  return (
    <html lang="pt-BR">
      <body>
        <Providers>
          <nav className="navbar navbar-expand-lg bg-white shadow-sm">
            <div className="container">
              <a className="navbar-brand" href="/">
                NutriScan
              </a>
              <div className="d-flex gap-3 ms-auto">
                <a className="btn btn-link" href="/dashboard">
                  Dashboard
                </a>
              </div>
            </div>
          </nav>
          <main>{children}</main>
        </Providers>
      </body>
    </html>
  );
}
