import React from 'react'

const Dashboard = React.lazy(() => import('./views/dashboard/Dashboard'))
const AuditAdmin = React.lazy(() => import('./views/administration/AuditAdmin'))
const GeographyList = React.lazy(() => import('./views/administration/GeographyList'))
const GeographyEdit = React.lazy(() => import('./views/administration/GeographyEdit'))

const routes = [
  { path: '/', exact: true, name: 'Home' },
  { path: '/dashboard', name: 'Dashboard', element: Dashboard },
  { path: '/administration/audit', name: 'Audit Records', element: AuditAdmin },
  { path: '/administration/geographies', name: 'Geographies', element: GeographyList },
  { path: '/administration/geographies/new', name: 'Add Geography', element: GeographyEdit },
  { path: '/administration/geographies/:id', name: 'Edit Geography', element: GeographyEdit },
]

export default routes
