import React from 'react'
import CIcon from '@coreui/icons-react'
import { cilSpeedometer, cilShieldAlt, cilGlobeAlt } from '@coreui/icons'
import { CNavItem, CNavTitle } from '@coreui/react'

interface Badge {
  color: string
  text: string
}

interface NavItem {
  component: typeof CNavItem | typeof CNavTitle
  name?: string
  to?: string
  icon?: React.ReactElement
  badge?: Badge
  items?: NavItem[]
}

const _nav: NavItem[] = [
  {
    component: CNavItem,
    name: 'Dashboard',
    to: '/dashboard',
    icon: <CIcon icon={cilSpeedometer} customClassName="nav-icon" />,
  },
  {
    component: CNavTitle,
    name: 'Administration',
  },
  {
    component: CNavItem,
    name: 'Geographies',
    to: '/administration/geographies',
    icon: <CIcon icon={cilGlobeAlt} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Audit Records',
    to: '/administration/audit',
    icon: <CIcon icon={cilShieldAlt} customClassName="nav-icon" />,
  },
]

export default _nav
